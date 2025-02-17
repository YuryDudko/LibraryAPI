using Domain.Entities;
using LibraryWebApi.Data;
using LibraryWebApi.DTO;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApi.Services.AuthorService;

public class AuthorRepository : IAuthorRepository
{
    private readonly AppDbContext _context;

    public AuthorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAllAuthorsAsync(int pageNumber, int pageSize)
    {
        return await _context.Authors
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Author?> GetAuthorByIdAsyncNormal(int id)
    {
        return await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<AuthorDto?> GetAuthorByIdAsync(int id)
    {
        var author = await _context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (author == null)
            return null;

        return new AuthorDto
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            DateOfBirth = author.DateOfBirth,
            Country = author.Country,
            BookTitles = author.Books.Select(b => b.Title).ToList()
        };
    }


    public async Task AddAuthorAsync(Author author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAuthorAsync(Author author)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAuthorAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author != null)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId)
    {
        return await _context.Books
            .Where(b => b.AuthorId == authorId)
            .ToListAsync();
    }
}
