using MediatR;
using Microsoft.EntityFrameworkCore;
using Newsletter.Domain.Repositories;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Newsletter.Domain.Events;
public sealed class SendQueueSubscribers(
    ISubscriberRepository subscriberRepository) : INotificationHandler<BlogEvent>
{
    public async Task Handle(BlogEvent notification, CancellationToken cancellationToken)
    {
        //Creating the connection.
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        //Queue is being created.
        channel.QueueDeclare(
            queue: "newsletter2",
            exclusive: false,
            autoDelete: false,
            arguments: null);

        List<string> emails = await subscriberRepository.Where(p => p.EmailConfirmed).Select(s => s.Email).ToListAsync();
        foreach (var email in emails)
        {
            var data = new
            {
                Email = email,
                BlogId = notification.BlogId
            };

            //We will send it to the queue.

            //We first convert the data to string.
            string message = JsonSerializer.Serialize(data);

            //We convert from string to byte. The body we will send to the queue is ready.      
            var body = Encoding.UTF8.GetBytes(message);

            //We send it to the queue.
            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "newsletter2",
                basicProperties: null,
                body: body);

            Console.WriteLine($"[x] {email} sent queue");
        }
        await Task.CompletedTask;
    }
}
