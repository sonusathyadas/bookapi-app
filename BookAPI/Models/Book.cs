namespace BookAPI.Models;
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ImageUrl { get; set; }
    public string Language { get; set; }
    public string Category { get; set; }
    public int PublishedYear { get; set; }
}