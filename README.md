# Newsletter Application
This project is a Newsletter application developed using .NET MVC. The primary goal is to learn the usage of RabbitMQ and apply software development techniques such as Clean Architecture, CQRS, and Domain Events during the process. Different consumer structures integrated with RabbitMQ, such as Console Application and Background Service, were utilized during development. The role and advantages of each within the system were observed. For those who wish to learn and understand these structures in-depth, the codes demonstrating their usage are available in this project for review.

## Technologies Used
- **MVC**: Model-View-Controller architecture.
- **CQRS Pattern**: Command Query Responsibility Segregation segregates command and query operations.
- **Clean Architecture**: Ensures low dependencies and independence through a layered architecture.
- **RabbitMQ**: Message queuing management.
- **Result Pattern**: Standard structure related to the returns from operations.
- **Domain Event**: Management of domain events.
- **MS SQL**: Database management.

## Libraries Used
- **Identity**: For authentication and authorization processes.
- **MediatR**: Used as a mediator in CQRS pattern implementations.
- **AutoMapper**: For automatic type conversions between objects.
- **CTS.Result**: Provides a standard format for operation results.
- **Scrutor**: Automatically registers services by scanning assemblies, simplifying dependency injection setup.
- **Bogus**: To generate fake data for the subscriber list.
- **FluentEmail.Smtp (smtp4dev )**: Used for testing mail sending during development.

## Features

### User Login
- **Authentication**: Cookie-based authentication system.
- **Pages**: Home, Login, and Newsletter pages.
- **User Management**: Automatically assigned a user; redirection implemented for unauthorized page access.

### Newsletter Management - Consumer: Console App
- Adding new blogs.
- Seeding fake data into the Subscriber list using SeedData.
- Creating a Queue for sending completed blogs as emails. Example: Once a blog is published, 1000 emails are queued.
- DomainEvent was used in the process of publishing the blog post.
- Added a project to listen to the queue. "Newsletter.Consumer" ConsoleApp.
- A fake mail structure (smtp4dev) was used to send mail.

### Newsletter Management - Consumer: BackgroundService
- A Checkbox (ChangeStatus) has been added to pull the unpublished blog post to Publish and change it.
- A Background Service was written to listen to the Queue. (Service Tool was used for Dependency Injection.)

##

### Asynchronous Communication with RabbitMQ
RabbitMQ plays a critical role to provide asynchronous communication in the project. For example, data or information created in one part of an application is sent using RabbitMQ to be processed in another part. This process occurs asynchronously; That is, after one part finishes its work and sends the message, the other part starts processing when it receives the message and is ready to process it. This allows the part that sent the message to continue its own operations and not wait for the message to be received and processed.

### RabbitMQ (Consumer) Structures
Two different consumer structures are used to process messages within the project:
1. **Console Application**: This approach works while the application is open and stops when the application is closed, used for simple and rapid message processing.
2. **Background Service**: This approach operates independently of the application, preferred for situations requiring continuous service.

Both consumer types are used for message processing and differ based on their usage purposes. The Console Application is suitable for testing and simple applications, while the Background Service is better suited for continuous operation and requires more management.

### Advantages of Using RabbitMQ
1. **Performance Increase**: Different parts of the application do not have to wait for each other, significantly improving overall performance.
2. **Resource Utilization Optimization**: Asynchronous communication allows for more efficient use of system resources and more processes to be executed in parallel.
3. **Resilience and Error Tolerance**: Errors or delays in one part do not affect other parts, enhancing the application's overall resilience and simplifying error management.

## Test Instructions
To test the project successfully, follow these steps:

### For Consumer as Console Application (Newsletter.Consumer) 
- **Run the Application**: Start the application in your local development environment.
- **Seed Data Loading**: You can load test data to the database using Postman.
- **Add Blog**: Add a new blog post through the application.
- **Check Message Queue**: Check the message Queue from the RabbitMQ Admin Panel and verify that the newly added blog is added to the Queue properly.
- **Run Consumer Console Application**: Use `dotnet run` in PowerShell or set up multiple startup projects in Visual Studio to start both `Newsletter.Consumer` and `Newsletter.MVC`.
- **SMTP Check**: Use smtp4dev to verify whether the mails have been sent successfully.

### For Consumer as Background Service
- Background Service starts automatically as part of the application. If manual start is necessary, initiate the relevant service.
- **Publish Blog**: Add a new blog post and publish it. Background Service will automatically trigger relevant actions when the blog is published.
- **Check Email Sending**: Examine the SMTP server logs to check whether e-mail sending operations triggered by Background Service are successful.
