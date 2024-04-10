using AcmeCorp.Domain.Interfaces.Databases;
using AcmeCorp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;

namespace AcmeCorp.Infrastructure.Database
{
    public class AcmeDbContext : DbContext, IAcmeDbContext
    {
        public AcmeDbContext(DbContextOptions<AcmeDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        DatabaseFacade IAcmeDbContext.Database { get => this.Database; }

        public string JsonValue(string column, [NotParameterized] string path)
     => throw new NotSupportedException();

        public string JsonQuery(string column, [NotParameterized] string path) =>
            throw new NotSupportedException();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_address");

                entity.ToTable("address");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id");
                entity.Property(e => e.Type)
                    .HasColumnName("type");
                entity.Property(e => e.Street1)
                    .HasMaxLength(128)
                    .HasColumnName("street_1");
                entity.Property(e => e.Street2)
                    .HasMaxLength(128)
                    .HasColumnName("street_2");
                entity.Property(e => e.City)
                    .HasMaxLength(64)
                    .HasColumnName("city");
                entity.Property(e => e.StateProvince)
                    .HasMaxLength(64)
                    .HasColumnName("state_province");
                entity.Property(e => e.PostalCode)
                    .HasMaxLength(16)
                    .HasColumnName("postal_code");
                entity.Property(e => e.Country)
                    .HasMaxLength(64)
                    .HasColumnName("country");
                entity.Property(e => e.Archive)
                    .HasDefaultValue(false)
                    .HasColumnName("archive");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(64)
                    .HasColumnName("created_by");
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");
                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(64)
                    .HasColumnName("modified_by");
                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_date");

                entity.HasOne(e => e.Customer).WithMany(e => e.Addresses)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_address_customer");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_customer");

                entity.ToTable("customer");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                entity.Property(e => e.FirstName)
                    .HasMaxLength(64)
                    .HasColumnName("first_name");
                entity.Property(e => e.MiddleName)
                    .HasMaxLength(64)
                    .HasColumnName("middle_name");
                entity.Property(e => e.LastName)
                    .HasMaxLength(64)
                    .HasColumnName("last_name");
                entity.Property(e => e.Archive)
                    .HasDefaultValue(false)
                    .HasColumnName("archive");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(64)
                    .HasColumnName("created_by");
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");
                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(64)
                    .HasColumnName("modified_by");
                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_date");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_order");

                entity.ToTable("order");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id");
                entity.Property(e => e.Status)
                    .HasColumnName("status");
                entity.Property(e => e.ShipMethod)
                    .HasMaxLength(16)
                    .HasColumnName("ship_method");
                entity.Property(e => e.DateShipped)
                    .HasColumnType("datetime")
                    .HasColumnName("date_shipped");
                entity.Property(e => e.Archive)
                    .HasDefaultValue(false)
                    .HasColumnName("archive");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(64)
                    .HasColumnName("created_by");
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");
                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(64)
                    .HasColumnName("modified_by");
                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_date");

                entity.HasOne(e => e.Customer).WithMany(e => e.Orders)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_customer");

            });
        }
    }
}
