
using Domain.Entities;
using LibraryWebApi.Data;
using LibraryWebApi.Services.AuthorService;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class AuthorRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly AuthorRepository _authorRepository;

    public AuthorRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _authorRepository = new AuthorRepository(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetAllAuthorsAsync_ReturnsCorrectNumberOfAuthors()
    {
        // Arrange
        _context.Authors.AddRange(
            new Author { Id = 1, FirstName = "Author", LastName = "One" },
            new Author { Id = 2, FirstName = "Author", LastName = "Two" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _authorRepository.GetAllAuthorsAsync(1, 10);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAuthorByIdAsyncNormal_ReturnsCorrectAuthor()
    {
        // Arrange
        var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        // Act
        var result = await _authorRepository.GetAuthorByIdAsyncNormal(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
    }

    [Fact]
    public async Task GetAuthorByIdAsync_ReturnsCorrectAuthorWithBooks()
    {
        // Arrange
        var author = new Author { Id = 1, FirstName = "Jane", LastName = "Doe" };
        var book1 = new Book { Id = 1, Title = "Book One", AuthorId = 1 };
        var book2 = new Book { Id = 2, Title = "Book Two", AuthorId = 1 };
        _context.Authors.Add(author);
        _context.Books.AddRange(book1, book2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _authorRepository.GetAuthorByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Jane", result.FirstName);
        Assert.Equal(2, result.BookTitles.Count);
        Assert.Contains("Book One", result.BookTitles);
        Assert.Contains("Book Two", result.BookTitles);
    }

    [Fact]
    public async Task AddAuthorAsync_AddsAuthorToDatabase()
    {
        // Arrange
        var author = new Author { Id = 1, FirstName = "New", LastName = "Author" };

        // Act
        await _authorRepository.AddAuthorAsync(author);
        var result = await _context.Authors.FirstOrDefaultAsync(a => a.Id == 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New", result.FirstName);
        Assert.Equal("Author", result.LastName);
    }

    [Fact]
    public async Task UpdateAuthorAsync_UpdatesAuthorInDatabase()
    {
        // Arrange
        var author = new Author { Id = 1, FirstName = "Old", LastName = "Name" };
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        author.FirstName = "Updated";
        author.LastName = "Name Updated";

        // Act
        await _authorRepository.UpdateAuthorAsync(author);
        var result = await _context.Authors.FirstOrDefaultAsync(a => a.Id == 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated", result.FirstName);
        Assert.Equal("Name Updated", result.LastName);
    }

    [Fact]
    public async Task DeleteAuthorAsync_RemovesAuthorFromDatabase()
    {
        // Arrange
        var author = new Author { Id = 1, FirstName = "Delete", LastName = "Me" };
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        // Act
        await _authorRepository.DeleteAuthorAsync(1);
        var result = await _context.Authors.FirstOrDefaultAsync(a => a.Id == 1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetBooksByAuthorAsync_ReturnsCorrectBooks()
    {
        // Arrange
        var author = new Author { Id = 1, FirstName = "George", LastName = "Martin" };
        var book1 = new Book { Id = 1, Title = "A Game of Thrones", AuthorId = 1 };
        var book2 = new Book { Id = 2, Title = "A Clash of Kings", AuthorId = 1 };
        _context.Authors.Add(author);
        _context.Books.AddRange(book1, book2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _authorRepository.GetBooksByAuthorAsync(1);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, b => b.Title == "A Game of Thrones");
        Assert.Contains(result, b => b.Title == "A Clash of Kings");
    }
}
