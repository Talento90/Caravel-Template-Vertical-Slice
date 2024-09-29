using Caravel.Functional;
using CaravelTemplate.Api.Shared.Data;
using CaravelTemplate.Api.Shared.Domain.Books;
using CaravelTemplate.Api.Shared.Messaging;
using CaravelTemplate.Api.Shared.Metrics;
using Mapster;
using MediatR;
using IPublisher = CaravelTemplate.Api.Shared.Messaging.IPublisher;
using Messaging_IPublisher = CaravelTemplate.Api.Shared.Messaging.IPublisher;

namespace CaravelTemplate.Api.Features.Books.CreateBook;

public sealed class CreateBookHandler : IRequestHandler<CreateBookRequest, Result<CreateBookResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly BookMetrics _metrics;
    private readonly Messaging_IPublisher _publisher;

    public CreateBookHandler(IUnitOfWork unitOfWork, BookMetrics metrics, Messaging_IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _metrics = metrics;
        _publisher = publisher;
    }

    public async Task<Result<CreateBookResponse>> Handle(CreateBookRequest request, CancellationToken ct)
    {
        var book = new Book(request.Name, request.Description);
        var result = await _unitOfWork.BookRepository.CreateBookAsync(book, ct);

        await _unitOfWork.SaveChangesAsync(ct);

        if (!result.IsSuccess)
        {
            return Result<CreateBookResponse>.Failure(result.Error);
        }

        await _publisher.PublishAsync(new BookCreatedMessage(book.Id, book), ct);
        _metrics.IncrementBookCreated(book.Id);
        return Result<CreateBookResponse>.Success(book.Adapt<CreateBookResponse>());
    }
}