using Caravel.AspNetCore.Endpoint;
using Caravel.AspNetCore.Http;
using Caravel.Functional;
using MediatR;

namespace CaravelTemplate.Api.Features.Books.GetBookById;

public class GetBookByIdEndpoint : IEndpointFeature
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("books/{bookId}", async (Guid bookId, ISender sender, CancellationToken ct) =>
            {
                var request = new GetBookByIdRequest(bookId);
                var result = await sender.Send(request, ct);
                return result.Map(Results.Ok, err => err.ToApiProblemDetailsResult());
            })
            .WithName(nameof(GetBookById))
            .WithDescription("Get a book by it's unique identifier.")
            .WithTags(Tags.Books)
            .Produces<GetBookByIdResponse>(200)
            .Produces<ApiProblemDetails>(400)
            .Produces<ApiProblemDetails>(500)
            .WithOpenApi();
    }
}