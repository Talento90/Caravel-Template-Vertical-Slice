using CaravelTemplate.Api.Features.Books.GetBookById;
using CaravelTemplate.Api.Shared.Domain.Books;
using Mapster;

namespace CaravelTemplate.Api.Features.Books;

public static class Mappings
{
    public static void Map()
    {
        TypeAdapterConfig<Book, GetBookByIdRequest>.NewConfig()
            .Map(dest => dest.Id, src => src.Id); // Example how to map custom properties.
    }
}