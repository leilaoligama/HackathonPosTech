using HackathonPosTech.Domain.Dtos;
using HackathonPosTech.Domain.Interfaces;
using HackathonPosTech.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HackathonPosTech.Domain.Common;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IMessageProcessorService<EmailMessageDto>, EmailMessageProcessorService>();
        services.AddScoped<IMessageProcessorService<UploadMessageDto>, UploadMessageProcessorService>();

        return services;
    }
}