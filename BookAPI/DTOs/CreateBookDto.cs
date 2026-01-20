namespace BookAPI.DTOs
{
    public class CreateBookDto
    {
    public required string Title { get; set; }
    public required string Author { get; set; }
    public string? ImageUrl { get; set; }
    public required string Language { get; set; }
    public required string Category { get; set; }
    public int PublishedYear { get; set; }
    }
}