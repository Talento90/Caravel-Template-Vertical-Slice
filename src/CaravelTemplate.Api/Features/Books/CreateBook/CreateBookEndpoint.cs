using Caravel.AspNetCore.Endpoint;
using Caravel.AspNetCore.Http;
using Caravel.Functional;
using MediatR;

namespace CaravelTemplate.Api.Features.Books.CreateBook;

public class CreateBookEndpoint : IEndpointFeature
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("books",
                async (ISender sender, CreateBookRequest command, CancellationToken ct) =>
                {
                    var result = await sender.Send(command, ct);
                    return result.Map(book => Results.Created($"/api/v1/books/{book.Id}", book),
                        err => err.ToApiProblemDetailsResult());
                })
            .WithName(nameof(CreateBookEndpoint))
            .WithDescription("Create a new book.")
            .WithTags(Tags.Books)
            .Produces<CreateBookResponse>(201)
            .Produces<ApiProblemDetails>(400)
            .Produces<ApiProblemDetails>(500)
            .WithOpenApi();
    }
}