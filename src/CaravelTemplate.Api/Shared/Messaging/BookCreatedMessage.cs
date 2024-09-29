using CaravelTemplate.Api.Shared.Domain.Books;

namespace CaravelTemplate.Api.Shared.Messaging;

public record BookCreatedMessage(Guid Id, Book Book) : IMessage;
