using Domain.Entities;
using LibraryWebApi.Data;
using LibraryWebApi.Services.BookService;
using Microsoft.EntityFrameworkCore;

public class BookRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly BookRepository _bookRepository;

    public BookRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _bookRepository = new BookRepository(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetAllBooksAsyncNormal_ReturnsCorrectNumberOfBooks()
    {
        // Arrange
        _context.Books.AddRange(
            new Book { Id = 1, ISBN = "123", Title = "Book 1" },
            new Book { Id = 2, ISBN = "456", Title = "Book 2" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _bookRepository.GetAllBooksAsyncNormal(1, 10);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetBookByIdAsyncNormal_ReturnsCorrectBook()
    {
        // Arrange
        var book = new Book { Id = 1, ISBN = "123", Title = "Test Book" };
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        // Act
        var result = await _bookRepository.GetBookByIdAsyncNormal(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Book", result.Title);
    }

    [Fact]
    public async Task GetBookByISBNAsyncNormal_ReturnsCorrectBook()
    {
        // Arrange
        var book = new Book { Id = 1, ISBN = "123-456", Title = "ISBN Test" };
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        // Act
        var result = await _bookRepository.GetBookByISBNAsyncNormal("123-456");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ISBN Test", result.Title);
    }

    [Fact]
    public async Task AddBookAsync_AddsBookToDatabase()
    {
        // Arrange
        var book = new Book { Id = 1, ISBN = "789", Title = "New Book" };

        // Act
        await _bookRepository.AddBookAsync(book);
        var result = await _context.Books.FirstOrDefaultAsync(b => b.Id == 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Book", result.Title);
    }

    [Fact]
    public async Task UpdateBookAsync_UpdatesBookInDatabase()
    {
        // Arrange
        var book = new Book { Id = 1, ISBN = "101", Title = "Old Title" };
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        book.Title = "Updated Title";

        // Act
        await _bookRepository.UpdateBookAsync(book);
        var result = await _context.Books.FirstOrDefaultAsync(b => b.Id == 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
    }

    [Fact]
    public async Task DeleteBookAsync_RemovesBookFromDatabase()
    {
        // Arrange
        var book = new Book { Id = 1, ISBN = "202", Title = "To Be Deleted" };
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        // Act
        await _bookRepository.DeleteBookAsync(1);
        var result = await _context.Books.FirstOrDefaultAsync(b => b.Id == 1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task BorrowBookAsync_SetsBorrowedDates()
    {
        // Arrange
        var book = new Book { Id = 1, ISBN = "303", Title = "Borrowed Book" };
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        // Act
        await _bookRepository.BorrowBookAsync(1, 42);
        var result = await _context.Books.FirstOrDefaultAsync(b => b.Id == 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.BorrowedAt > DateTime.MinValue);
        Assert.True(result.ReturnBy > result.BorrowedAt);
    }

    [Fact]
    public async Task AddBookImageAsync_SavesImageToBook()
    {
        // Arrange
        var book = new Book { Id = 1, ISBN = "404", Title = "With Image" };
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        var imageData = new byte[] { 1, 2, 3, 4 };

        // Act
        await _bookRepository.AddBookImageAsync(1, imageData);
        var result = await _context.Books.FirstOrDefaultAsync(b => b.Id == 1);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.ImageData);
        Assert.Equal(imageData, result.ImageData);
    }
}
