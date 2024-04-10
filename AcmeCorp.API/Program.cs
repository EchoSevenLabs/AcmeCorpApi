using AcmeCorp.API.Authentication;
using AcmeCorp.Domain.Interfaces.Databases;
using AcmeCorp.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using AcmeCorp.Infrastructure.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AcmeDbContext>(options =>  
                                                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                                                x => x.MigrationsAssembly("AcmeCorp.Infrastructure")));

builder.Services.AddScoped<IAcmeDbContext>(provider => provider.GetRequiredService<AcmeDbContext>());
builder.Services.AddAutoMapper(Assembly.Load("AcmeCorp.Infrastructure"));
builder.Services.AddInfrastructureServices();

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = false; });
builder.Services.AddAuthentication("ApiKey").AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s => { s.UseInlineDefinitionsForEnums(); });

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var logger = services.GetRequiredService<ILogger<Program>>();
//    try
//    {
//        var dbContext = services.GetRequiredService<AcmeDbContext>();
//        logger.LogInformation("Applying database migrations...");
//        dbContext.Database.Migrate();
//        logger.LogInformation("Database migrations applied successfully.");
//    }
//    catch (Exception ex)
//    {
//        logger.LogError(ex, "An error occurred while applying database migrations.");
//    }
//}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }