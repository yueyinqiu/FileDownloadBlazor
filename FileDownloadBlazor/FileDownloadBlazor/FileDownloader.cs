using Microsoft.JSInterop;

namespace FileDownloadBlazor;

internal sealed class FileDownloader(IJSRuntime jsRuntime) : IAsyncDisposable, IFileDownloader
{
    private readonly Task<IJSObjectReference> moduleTask = 
        jsRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            "./_content/FileDownloadBlazor/downloader.js").AsTask();
    public async Task DownloadAsync(
        string uri, string fileName = "",
        CancellationToken cancellationToken = default)
    {
        var module = await moduleTask;
        await module.InvokeVoidAsync("downloadFromUri", cancellationToken, [fileName, uri]);
    }
    public async Task DownloadAsync(
        byte[] bytes, string fileName = "",
        CancellationToken cancellationToken = default)
    {
        var module = await moduleTask;
        await module.InvokeVoidAsync("downloadBytes", cancellationToken, [fileName, bytes]);
    }
    public async Task DownloadAsync(
        Stream stream, string fileName = "", bool leaveOpen = true,
        CancellationToken cancellationToken = default)
    {
        var module = await moduleTask;
        using var reference = new DotNetStreamReference(stream, leaveOpen);
        await module.InvokeVoidAsync("downloadStream", cancellationToken, [fileName, reference]);
    }
    public async ValueTask DisposeAsync()
    {
        var module = await moduleTask;
        await module.DisposeAsync();
    }
}
