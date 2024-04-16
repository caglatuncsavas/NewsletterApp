using Bogus;
using GenericRepository;
using Microsoft.AspNetCore.Mvc;
using Newsletter.Domain.Entities;
using Newsletter.Domain.Repositories;

namespace Newsletter.MVC.Controllers;
[Route("api/[controller]")]
[ApiController]
public sealed class SeedDataController(
    ISubscriberRepository subscriberRepository,
    IBlogRepository blogRepository,
    IUnitOfWork unitOfWork) : ControllerBase
{
    public async Task<IActionResult> Seed(CancellationToken cancellationToken)
    {
        //Blog yazısı ekliyoruz.
        //List<Blog> blogs = new();
        //for(int i = 0; i < 5; i++)
        //{
        //   Faker faker = new();
        //   Random random = new();

        //    Blog blog = new()
        //    {
        //        Title = faker.Lorem.Letter(random.Next(5, 8)),
        //        Summary= faker.Lorem.Letter(random.Next(15, 55)),
        //        Content = faker.Lorem.Lines(random.Next(3, 7), "<br><br>"),
        //        IsPublish = (i% 2 == 0),
        //        PublishDate = (i % 2 == 0 ? DateOnly.FromDateTime(DateTime.Now) : null)
        //    };
        //    blogs.Add(blog);
        //}

        //await blogRepository.AddRangeAsync(blogs, cancellationToken);

        List<Subscriber> subscribers = new();

        for (int i = 0; i < 10; i++)
        {
            Faker faker = new();

            Subscriber subscriber = new()
            {
                Email = faker.Person.Email,
                EmailConfirmed = true
            };
            subscribers.Add(subscriber);
        }

        await subscriberRepository.AddRangeAsync(subscribers, cancellationToken);
       
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return NoContent();
     }
}
