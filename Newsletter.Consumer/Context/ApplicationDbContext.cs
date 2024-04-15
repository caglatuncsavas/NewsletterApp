using Microsoft.EntityFrameworkCore;
using Newsletter.Consumer.Models;

namespace Newsletter.Consumer.Context;
internal sealed class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source = CAGLA\\SQLEXPRESS; Initial Catalog = NewsletterDb; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate = False; Application Intent = ReadWrite; Multi Subnet Failover = False");
    }

    public DbSet<Blog> Blogs { get; set; }
}
