using Domain.Entities;
using LibraryWebApi.Services.BookService;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _bookRepository;

    public BooksController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var books = await _bookRepository.GetAllBooksAsync(pageNumber, pageSize);
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);
        return book != null ? Ok(book) : NotFound();
    }

    [HttpGet("isbn/{isbn}")]
    public async Task<IActionResult> GetBookByISBN(string isbn)
    {
        var book = await _bookRepository.GetBookByISBNAsync(isbn);
        return book != null ? Ok(book) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] Book book)
    {
        await _bookRepository.AddBookAsync(book);
        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
    {
        var existingBook = await _bookRepository.GetBookByIdAsyncNormal(id);
        if (existingBook == null) return NotFound();

        existingBook.ISBN = book.ISBN;
        existingBook.Author = book.Author;
        existingBook.AuthorId = book.AuthorId;
        existingBook.BorrowedAt = book.BorrowedAt;
        existingBook.ReturnBy = book.ReturnBy;
        existingBook.Genre = book.Genre;
        existingBook.Title = book.Title;
        existingBook.Description = book.Description;    

        await _bookRepository.UpdateBookAsync(existingBook);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        await _bookRepository.DeleteBookAsync(id);
        return NoContent();
    }

    [HttpPost("{bookId}/borrow/{userId}")]
    public async Task<IActionResult> BorrowBook(int bookId, int userId)
    {
        await _bookRepository.BorrowBookAsync(bookId, userId);
        return NoContent();
    }

    [HttpPost("{bookId}/upload-image")]
    public async Task<IActionResult> UploadBookImage(int bookId, [FromForm] IFormFile image)
    {
        if (image == null || image.Length == 0)
            return BadRequest("Invalid image file.");

        using var memoryStream = new MemoryStream();
        await image.CopyToAsync(memoryStream);
        byte[] imageData = memoryStream.ToArray();

        await _bookRepository.AddBookImageAsync(bookId, imageData);
        return Ok("Image uploaded successfully.");
    }

}
