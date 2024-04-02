namespace Newsletter.Domain.Entities;

public sealed class Blog
{
    public Blog()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime? PublishedDate { get; set; }
    public bool IsPublished { get; set; }
}
