namespace LibraryWebApi.DTO;

public class BookDto
{
    public int Id { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public AuthorDto Author { get; set; } = null!;
    public DateTime BorrowedAt { get; set; }
    public DateTime ReturnBy { get; set; }
}
