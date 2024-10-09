using System.Reflection;

namespace CaravelTemplate.Api.Extensions;

public static class OpenApiExtensions
{
    public static void AddOpenApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen((options) =>
        {
            options.SupportNonNullableReferenceTypes();
            
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }
}