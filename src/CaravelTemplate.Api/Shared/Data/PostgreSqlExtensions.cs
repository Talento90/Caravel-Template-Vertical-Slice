using CaravelTemplate.Api.Shared.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CaravelTemplate.Api.Shared.Data;

public static class PostgreSqlExtensions
{
    public static void RegisterPostgreSql(this IServiceCollection services, PostgreSqlOptions options)
    {
        services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(options.ConnectionString);
        });

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}