using Domain.Entities;
using LibraryWebApi.DTO;

namespace LibraryWebApi.Services.BookService;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllBooksAsyncNormal(int pageNumber, int pageSize);
    Task<Book?> GetBookByIdAsyncNormal(int id);
    Task<Book?> GetBookByISBNAsyncNormal(string isbn);
    Task AddBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(int id);
    Task BorrowBookAsync(int bookId, int userId);
    Task AddBookImageAsync(int bookId, byte[] imageData);
    Task<IEnumerable<BookDto>> GetAllBooksAsync(int pageNumber, int pageSize);
    Task<BookDto?> GetBookByIdAsync(int id);
    Task<BookDto?> GetBookByISBNAsync(string isbn);
}
