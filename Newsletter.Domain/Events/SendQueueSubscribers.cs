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
        //Bağlantı oluşturuluyor.
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        //Kuyruk oluşturuluyor.
        channel.QueueDeclare(
            queue: "newsletter",
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

            //kuyruğa göndereceğiz.

            //Datayı önce stringe çeviriyoruz.
            string message = JsonSerializer.Serialize(data);

            //stringden de byte'a çeviriyoruz.  Kuyruğa göndereceğimiz body hazır.       
            var body = Encoding.UTF8.GetBytes(message);

            //Kuyruğa gönderiyoruz.
            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "newsletter",
                basicProperties: null,
                body: body);

            Console.WriteLine($"[x] {email} sent queue");
        }
        await Task.CompletedTask;
    }
}
