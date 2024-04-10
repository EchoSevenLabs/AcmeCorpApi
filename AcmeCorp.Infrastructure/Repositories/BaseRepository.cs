using AcmeCorp.Domain.Interfaces.Models;
using AcmeCorp.Domain.Interfaces.Repositories;
using AcmeCorp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IAuditable
    {
        protected readonly AcmeDbContext _context;

        public BaseRepository(AcmeDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(bool includeArchived = false)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (!includeArchived && typeof(IAuditable).IsAssignableFrom(typeof(TEntity)))
            {
                ParameterExpression param = Expression.Parameter(typeof(TEntity), "e");
                Expression propAccess = Expression.Property(param, "Archive");
                Expression notArchived = Expression.Not(propAccess);
                Expression<Func<TEntity, bool>> predicate = Expression.Lambda<Func<TEntity, bool>>(notArchived, param);

                query = query.Where(predicate);
            }

            return await query.ToListAsync();

        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        async Task<TEntity> IBaseRepository<TEntity>.CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        async Task IBaseRepository<TEntity>.UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            _context.SaveChanges();
        }
    }
}
