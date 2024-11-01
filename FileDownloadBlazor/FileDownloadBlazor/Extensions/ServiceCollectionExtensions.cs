using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FileDownloadBlazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileDownloadBlazor(
        this IServiceCollection services,
        ServiceLifetime lifeTime = ServiceLifetime.Scoped,
        object? key = null)
    {
        services.TryAdd(new ServiceDescriptor(
            typeof(IFileDownloader), key, typeof(FileDownloader), lifeTime));
        return services;
    }
}
