﻿using Microsoft.Extensions.DependencyInjection;
using Newsletter.Domain.Entities;

namespace Newsletter.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services
            .AddFluentEmail("admin@gmail.com")
            .AddSmtpSender("localhost", 2525);

        // services.AddHostedService<BlogBackgroundService>();

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly, typeof(Blog).Assembly);
        });

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
