using Domain.Entities;
using LibraryWebApi.Services.AuthorService;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorsController(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAuthors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var authors = await _authorRepository.GetAllAuthorsAsync(pageNumber, pageSize);
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthorById(int id)
    {
        var author = await _authorRepository.GetAuthorByIdAsync(id);
        return author != null ? Ok(author) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddAuthor([FromBody] Author author)
    {
        await _authorRepository.AddAuthorAsync(author);
        return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author author)
    {
        var existingAuthor = await _authorRepository.GetAuthorByIdAsyncNormal(id);
        if (existingAuthor == null) return NotFound();

        existingAuthor.DateOfBirth = author.DateOfBirth;
        existingAuthor.FirstName = author.FirstName;
        existingAuthor.LastName = author.LastName;
        existingAuthor.Country = author.Country;
        await _authorRepository.UpdateAuthorAsync(existingAuthor);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        await _authorRepository.DeleteAuthorAsync(id);
        return NoContent();
    }

    [HttpGet("{id}/books")]
    public async Task<IActionResult> GetBooksByAuthor(int id)
    {
        var books = await _authorRepository.GetBooksByAuthorAsync(id);
        return Ok(books);
    }
}
