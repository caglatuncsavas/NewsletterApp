using FluentEmail.Core;
using Microsoft.Extensions.DependencyInjection;
using Newsletter.Consumer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

var services = new ServiceCollection();

services.AddFluentEmail("info@gmail.com")
    .AddSmtpSender("localHost", 80);

var serviceProvider = services.BuildServiceProvider();
var fluentEmail = serviceProvider.GetRequiredService<IFluentEmail>();

var factory = new ConnectionFactory { HostName = "localHost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "newsletter",
    exclusive: false,
    autoDelete:false,
    arguments: null) ;

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(" [*] Waiting for messages....");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    ResponseDto? response = JsonSerializer.Deserialize<ResponseDto>(message);
    if (response is null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Repsonse is empty or null");
    }

    //mail gönderme işlemi yapılacak.
    fluentEmail
    .To(response.Email)
    .Subject()
};

channel.BasicConsume(
    queue: "newsletter",
    autoAck: true,
    consumer: consumer);

Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
