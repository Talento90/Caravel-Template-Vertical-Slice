using CaravelTemplate.Api.Shared.Domain.Books;
using CaravelTemplate.Api.Shared.Messaging;

namespace CaravelTemplate.Api.Features.Books.CreateBook;

public record BookCreatedMessage(Guid Id, Book Book) : IMessage;
