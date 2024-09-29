namespace CaravelTemplate.Api.Shared.Data;

public interface IUnitOfWork
{
    IBookRepository BookRepository { get; }
    Task SaveChangesAsync(CancellationToken ct);
}