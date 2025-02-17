using Domain.Entities;
using LibraryWebApi.Data;
using LibraryWebApi.DTO;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApi.Services.BookService;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsyncNormal(int pageNumber, int pageSize)
    {
        return await _context.Books
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Book?> GetBookByIdAsyncNormal(int id)
    {
        return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book?> GetBookByISBNAsyncNormal(string isbn)
    {
        return await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);
    }

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync(int pageNumber, int pageSize)
    {
        var books = await _context.Books
            .Include(b => b.Author)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return books.Select(b => new BookDto
        {
            Id = b.Id,
            ISBN = b.ISBN,
            Title = b.Title,
            Genre = b.Genre,
            Description = b.Description,
            BorrowedAt = b.BorrowedAt,
            ReturnBy = b.ReturnBy,
            Author = new AuthorDto
            {
                Id = b.Author.Id,
                FirstName = b.Author.FirstName,
                LastName = b.Author.LastName,
                DateOfBirth = b.Author.DateOfBirth,
                Country = b.Author.Country
            }
        });
    }

    public async Task<BookDto?> GetBookByIdAsync(int id)
    {
        var book = await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
            return null;

        return new BookDto
        {
            Id = book.Id,
            ISBN = book.ISBN,
            Title = book.Title,
            Genre = book.Genre,
            Description = book.Description,
            BorrowedAt = book.BorrowedAt,
            ReturnBy = book.ReturnBy,
            Author = new AuthorDto
            {
                Id = book.Author.Id,
                FirstName = book.Author.FirstName,
                LastName = book.Author.LastName,
                DateOfBirth = book.Author.DateOfBirth,
                Country = book.Author.Country
            }
        };
    }

    public async Task<BookDto?> GetBookByISBNAsync(string isbn)
    {
        var book = await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.ISBN == isbn);

        if (book == null)
            return null;

        return new BookDto
        {
            Id = book.Id,
            ISBN = book.ISBN,
            Title = book.Title,
            Genre = book.Genre,
            Description = book.Description,
            BorrowedAt = book.BorrowedAt,
            ReturnBy = book.ReturnBy,
            Author = new AuthorDto
            {
                Id = book.Author.Id,
                FirstName = book.Author.FirstName,
                LastName = book.Author.LastName,
                DateOfBirth = book.Author.DateOfBirth,
                Country = book.Author.Country
            }
        };
    }


    public async Task AddBookAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    public async Task BorrowBookAsync(int bookId, int userId)
    {
        var book = await _context.Books.FindAsync(bookId);
        if (book == null) return;

        book.BorrowedAt = DateTime.UtcNow;
        book.ReturnBy = DateTime.UtcNow.AddDays(14);

        await _context.SaveChangesAsync();
    }

    public async Task AddBookImageAsync(int bookId, byte[] imageData)
    {
        var book = await _context.Books.FindAsync(bookId);
        if (book == null) return;

        book.ImageData = imageData;
        await _context.SaveChangesAsync();
    }
}
