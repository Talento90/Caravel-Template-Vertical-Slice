using Caravel.Functional;
using CaravelTemplate.Api.Shared.Data;
using Mapster;
using MediatR;

namespace CaravelTemplate.Api.Features.Books.GetBookById;

public class GetBookByIdHandler : IRequestHandler<GetBookByIdRequest, Result<GetBookByIdResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBookByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetBookByIdResponse>> Handle(GetBookByIdRequest request, CancellationToken ct)
    {
        var result = await _unitOfWork.BookRepository.GetBookAsync(request.Id, ct);

        return result.Map(
            book => Result<GetBookByIdResponse>.Success(book.Adapt<GetBookByIdResponse>()),
            Result<GetBookByIdResponse>.Failure
        );
    }
}