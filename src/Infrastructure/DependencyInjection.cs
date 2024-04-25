using Application.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        var connectionString = configuration.GetConnectionString("ApplicationDb");

        services.AddScoped<ISaveChangesInterceptor, EntityDateInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, o ) =>
        {
            o.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            o.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        if (isDevelopment)
        {
            var dbContext = services.BuildServiceProvider().GetRequiredService<ApplicationDbContext>();

            // Using this only for the demo database, as we do not wish to create migrations for the demo
            dbContext.Database.EnsureCreated();

            // Replace when creating migrations have been created for a real database
            //dbContext.Database.Migrate();
        }

        return services;
    }
}