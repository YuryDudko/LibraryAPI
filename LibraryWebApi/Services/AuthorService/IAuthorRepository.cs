using Domain.Entities;
using LibraryWebApi.DTO;

namespace LibraryWebApi.Services.AuthorService;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAuthorsAsync(int pageNumber, int pageSize);
    Task<Author?> GetAuthorByIdAsyncNormal(int id);
    Task<AuthorDto?> GetAuthorByIdAsync(int id);
    Task AddAuthorAsync(Author author);
    Task UpdateAuthorAsync(Author author);
    Task DeleteAuthorAsync(int id);
    Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId);
}
