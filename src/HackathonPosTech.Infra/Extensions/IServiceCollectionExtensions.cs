using HackathonPosTech.Domain.Interfaces;
using HackathonPosTech.Infra.Database;
using HackathonPosTech.Infra.Database.Repositories;
using HackathonPosTech.Infra.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HackathonPosTech.Infra.Extensions;
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDatabase();
        services.AddHostedServices();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var configuration = provider.GetRequiredService<IConfiguration>();
        var sqlServerConnectionString = configuration["SqlServerConnectionString"];

        services.AddDbContextFactory<DatabaseContext>(options =>
        {
            options.UseSqlServer(sqlServerConnectionString);
            options.EnableSensitiveDataLogging(false);
        }).AddSqlServer<DatabaseContext>(sqlServerConnectionString);

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        return services;
    }

    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddScoped<IMessagePublisher, MessagePublisher>();
        services.AddScoped<IMessageProcessor, MessageProcessor>();

        return services;
    }
}