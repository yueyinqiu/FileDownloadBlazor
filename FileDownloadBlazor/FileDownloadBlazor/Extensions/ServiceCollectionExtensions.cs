using Microsoft.Extensions.DependencyInjection;

namespace FileDownloadBlazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileDownloadBlazor(
        this IServiceCollection services,
        ServiceLifetime lifeTime = ServiceLifetime.Scoped,
        object? key = null)
    {
        services.Add(new ServiceDescriptor(
            typeof(IFileDownloader), key, typeof(FileDownloader), lifeTime));
        return services;
    }
}
