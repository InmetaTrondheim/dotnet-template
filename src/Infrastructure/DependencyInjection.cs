using InmetaTemplate.Application.Common.Interfaces;
using InmetaTemplate.Infrastructure.Data;
using InmetaTemplate.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InmetaTemplate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        var connectionString = configuration.GetConnectionString("ApplicationDb");

        services.AddScoped<ISaveChangesInterceptor, EntityDateInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<InmetaTemplateDbContext>((sp, o ) =>
        {
            o.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            o.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<InmetaTemplateDbContext>());

        if (isDevelopment)
        {
            var dbContext = services.BuildServiceProvider().GetRequiredService<InmetaTemplateDbContext>();

            // Using this only for the demo database, as we do not wish to create migrations for the demo
            dbContext.Database.EnsureCreated();

            // Replace when creating migrations have been created for a real database
            //dbContext.Database.Migrate();
        }

        return services;
    }
}