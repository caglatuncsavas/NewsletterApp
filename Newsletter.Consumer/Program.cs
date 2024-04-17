using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Newsletter.Consumer;
using Newsletter.Consumer.Context;
using Newsletter.Consumer.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

var services = new ServiceCollection();

services
    .AddFluentEmail("info@gmail.com")
    .AddSmtpSender("localhost", 2525);

var serviceProvider = services.BuildServiceProvider();

ApplicationDbContext context = new();

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "newsletter",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(" [*] Waiting for messages....");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    //We turn the message into an object.
    ResponseDto? response = JsonSerializer.Deserialize<ResponseDto>(message);
    if (response is null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Repsonse is empty or null");
    }

    //sending email will be done.
    Blog? blog = context.Blogs.Find(response.BlogId);
    if(blog is null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Blog not found!");
    }
    var fluentEmail = serviceProvider.GetRequiredService<IFluentEmail>();
    SendResponse sendResponse = fluentEmail
        .To(response.Email)
        .Subject(blog!.Title)
        .Body(blog.Content, true)
        .Send();

    if(!sendResponse.Successful)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($" [*] tyr to {response.Email} blog sent but got an error");
    }
    else
    {

        Console.WriteLine($"Blog sent to {response.Email}");
    }
};

channel.BasicConsume(
    queue: "newsletter",
    autoAck: true,
    consumer: consumer);

Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
