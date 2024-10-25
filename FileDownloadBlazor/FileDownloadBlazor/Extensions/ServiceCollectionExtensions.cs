using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace FileDownloadBlazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileDownloadBlazor(
        this IServiceCollection services,
        bool synchronous = false,
        ServiceLifetime lifeTime = ServiceLifetime.Scoped,
        object? key = null)
    {
        if (synchronous)
        {
            services.Add(new ServiceDescriptor(
                typeof(IFileDownloaderSync),
                key,
                (services, _) =>
                {
                    var jsRuntime = (IJSInProcessRuntime)services.GetRequiredService<IJSRuntime>();
                    return new FileDownloaderSync(jsRuntime);
                },
                lifeTime));
        }
        else
        {
            services.Add(new ServiceDescriptor(
                typeof(IFileDownloader),
                key,
                (services, _) =>
                {
                    var jsRuntime = services.GetRequiredService<IJSRuntime>();
                    return new FileDownloader(jsRuntime);
                },
                lifeTime));
        }
        return services;
    }
}
