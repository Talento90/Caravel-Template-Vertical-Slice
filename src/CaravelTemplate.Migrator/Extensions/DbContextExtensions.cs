using CaravelTemplate.Api.Shared.Data;
using CaravelTemplate.Api.Shared.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CaravelTemplate.Migrator.Extensions;

public static class DbContextExtensions
{
    public static void AddApplicationDbContext(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var options = configuration
                          .GetSection("PostgreSql")
                          .Get<PostgreSqlOptions>()
                      ?? throw new NullReferenceException(nameof(PostgreSqlOptions));

        services.AddDbContext<ApplicationDbContext>((dbContextBuilder) =>
        {
            dbContextBuilder.UseNpgsql(options.ConnectionString, optionBuilder =>
            {
                optionBuilder.MigrationsHistoryTable(
                    tableName: HistoryRepository.DefaultTableName,
                    schema: ApplicationDbContext.Schema);

                optionBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });
        });
    }
    
    public static void AddIdentityDbContext(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var options = configuration
                                .GetSection("Identity")
                                .Get<IdentityOptions>()
                            ?? throw new NullReferenceException(nameof(IdentityOptions));

        services.AddDbContext<IdentityDbContext>((dbContextBuilder) =>
        {
            dbContextBuilder.UseNpgsql(options.ConnectionString, optionBuilder =>
            {
                optionBuilder.MigrationsHistoryTable(
                    tableName: HistoryRepository.DefaultTableName,
                    schema: IdentityDbContext.Schema);

                optionBuilder.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName);
            });
        });
    }
}