using Caravel.Functional;
using FluentValidation;
using MediatR;

namespace CaravelTemplate.Api.Features.Books.CreateBook;

public record CreateBookRequest(string Name, string Description) : IRequest<Result<CreateBookResponse>>
{
    public class Validator : AbstractValidator<CreateBookRequest>
    {
        public Validator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Description).NotEmpty();
        }
    }
}