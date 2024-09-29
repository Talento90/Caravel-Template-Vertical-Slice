using Caravel.Functional;
using FluentValidation;
using MediatR;

namespace CaravelTemplate.Api.Features.Books.GetBookById;

public record GetBookByIdRequest(Guid Id) : IRequest<Result<GetBookByIdResponse>>
{
    public class Validator : AbstractValidator<GetBookByIdRequest>
    {
        public Validator()
        {
            RuleFor(p => p.Id).NotEmpty();
        }
    }
}