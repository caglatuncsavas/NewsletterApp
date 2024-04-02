namespace Newsletter.Domain.Entities;

public sealed class Subscriber
{
    public Subscriber()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
}