using CaravelTemplate.Api.Shared.Data;
using CaravelTemplate.Api.Shared.Identity;
using CaravelTemplate.Api.Shared.Messaging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using IdentityOptions = CaravelTemplate.Api.Shared.Identity.IdentityOptions;

namespace CaravelTemplate.Api.Extensions;

public static class ServiceCollectionExtensions
{
    
    public static void AddFeatureManagement(this IServiceCollection services, IConfigurationManager configuration)
    {
        services
            .AddFeatureManagement(configuration.GetSection("FeatureManagement"))
            .AddFeatureFilter<PercentageFilter>();
    }
    
    public static void AddApplicationDbContext(
        this IServiceCollection services,
        IConfigurationManager configuration)
    {
        var postgreOption = configuration
            .GetSection("PostgreSql")
            .Get<PostgreSqlOptions>() ?? throw new NullReferenceException(nameof(PostgreSqlOptions));
        services.RegisterPostgreSql(postgreOption);
    }
    
    public static void AddIdentityDbContext(
        this IServiceCollection services,
        IConfigurationManager configuration)
    {
        var identityOption = configuration
            .GetSection("Identity")
            .Get<IdentityOptions>() ?? throw new NullReferenceException(nameof(IdentityOptions));
        services.RegisterIdentity(identityOption);
    }
    
    public static void AddMassTransit(
        this IServiceCollection services,
        IConfigurationManager configuration)
    {
        var massTransitOption = configuration
            .GetSection("MassTransit")
            .Get<MassTransitOptions>() ?? throw new NullReferenceException(nameof(MassTransitOptions));
        
        services.RegisterMassTransit(massTransitOption);
    }
}