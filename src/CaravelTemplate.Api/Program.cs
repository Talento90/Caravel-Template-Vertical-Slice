using System.Reflection;
using Asp.Versioning;
using Caravel.AspNetCore.Endpoint;
using Caravel.AspNetCore.Middleware;
using Caravel.AspNetCore.Security;
using Caravel.MediatR.Logging;
using Caravel.MediatR.Validation;
using Caravel.Security;
using CaravelTemplate.Adapter.Api.Extensions;
using CaravelTemplate.Api.Extensions;
using CaravelTemplate.Api.Shared.Metrics;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    var currentAssembly = Assembly.GetExecutingAssembly();

    builder.Services
        .AddFeatureManagement(builder.Configuration.GetSection("FeatureManagement"))
        .AddFeatureFilter<PercentageFilter>();
    
    
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    builder.Logging.ClearProviders();
    builder.Services.AddSerilog();

    builder.Services.AddSingleton<BookMetrics>();
    
    builder.Services.AddAdapterPostgreSql(builder.Configuration);
    builder.Services.AddIdentityAdapter(builder.Configuration);
    builder.Services.AddMassTransitAdapter(builder.Configuration);
    builder.Services.AddOpenTelemetry(builder.Configuration, builder.Environment);

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IUserContext, UserContext>();
    builder.Services.AddOpenApi();
    builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

    builder.Services.AddValidatorsFromAssembly(currentAssembly);
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(currentAssembly);
        cfg.AddOpenBehavior(typeof(LoggingPipelineBehaviour<,>));
        cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
    });

    builder.Services.AddEndpointFeatures(currentAssembly);
    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication()
        .AddBearerToken(IdentityConstants.BearerScheme);

    var application = builder.Build();

    // API Versioning
    var apiVersionSet = application.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .ReportApiVersions()
        .Build();

    var versionedGroup = application
        .MapGroup("api/v{version:apiVersion}")
        .WithApiVersionSet(apiVersionSet);

    // Configure the HTTP request pipeline.
    if (application.Environment.IsDevelopment())
    {
        application.UseSwagger();
        application.UseSwaggerUI();
    }

    application.UseSerilogRequestLogging();
    application.UseExceptionHandler();
    application.UseMiddleware<ActivityEnrichingMiddleware>();
    application.UseMiddleware<SecurityResponseHeadersMiddleware>();
    application.UseMiddleware<TraceIdResponseMiddleware>();

    application.UseHttpsRedirection();

    // Map the application endpoints
    application.MapEndpointFeatures(versionedGroup);

    Log.Information("Starting CaravelTemplate.Api");

    await application.StartAsync();
}
catch (Exception e)
{
    Log.Error(e, "Failed to start CaravelTemplate.Api");
}
finally
{
    await Log.CloseAndFlushAsync();
}


// This dummy class is needed for integration tests WebApplicationFactory
public partial class Program
{
}