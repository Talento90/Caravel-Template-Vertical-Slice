using CaravelTemplate.Api.Shared.Data;
using CaravelTemplate.Api.Shared.Identity;
using CaravelTemplate.Api.Shared.Messaging;

namespace CaravelTemplate.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAdapterPostgreSql(
        this IServiceCollection services,
        IConfigurationManager configuration)
    {
        var postgreOption = configuration
            .GetSection("PostgreSql")
            .Get<PostgreSqlOptions>() ?? throw new NullReferenceException(nameof(PostgreSqlOptions));
        services.RegisterPostgreSql(postgreOption);
    }
    
    public static void AddIdentityAdapter(
        this IServiceCollection services,
        IConfigurationManager configuration)
    {
        var identityOption = configuration
            .GetSection("Identity")
            .Get<IdentityOptions>() ?? throw new NullReferenceException(nameof(IdentityOptions));
        services.RegisterIdentity(identityOption);
    }
    
    public static void AddMassTransitAdapter(
        this IServiceCollection services,
        IConfigurationManager configuration)
    {
        var massTransitOption = configuration
            .GetSection("MassTransit")
            .Get<MassTransitOptions>() ?? throw new NullReferenceException(nameof(MassTransitOptions));
        
        services.RegisterMassTransit(massTransitOption);
    }
}