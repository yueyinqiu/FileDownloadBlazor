using Microsoft.JSInterop;
using System.Diagnostics;

namespace FileDownloadBlazor;

internal sealed class FileDownloader(IJSRuntime jsRuntime) 
    : IAsyncDisposable, IFileDownloader, ISyncFileDownloader
{
    public async ValueTask DisposeAsync()
    {
        var module = await moduleTask;
        await module.DisposeAsync();
    }

    private readonly Task<IJSObjectReference> moduleTask = 
        jsRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            "./_content/FileDownloadBlazor/FileDownloadBlazor.js").AsTask();

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

    public async Task CheckJavaScriptModuleAsync()
    {
        _ = await moduleTask;
    }

    private IJSInProcessObjectReference? syncModule = null;
    public ISyncFileDownloader Sync
    {
        get
        {
            if (this.syncModule is not null)
                return this;

            if (!moduleTask.IsCompleted)
            {
                throw new JSException(
                    "FileDownloadBlazor JavaScript module has not been imported yet. " +
                    "So you can't use it synchronously at present.");
            }
            if (!moduleTask.IsCompletedSuccessfully)
            {
                if (moduleTask.Exception is null)
                    throw new JSException(
                        "Failed to import FileDownloadBlazor JavaScript module.");
                else
                    throw new JSException(
                        "Failed to import FileDownloadBlazor JavaScript module.",
                        moduleTask.Exception);
            }
            if (moduleTask.Result is not IJSInProcessObjectReference syncModule)
            {
                throw new JSException(
                    "FileDownloadBlazor JavaScript module is not executed in process. " +
                    "So you can't use it synchronously.");
            }

            this.syncModule = syncModule;
            return this;
        }
    }

    public void Download(string uri, string fileName = "")
    {
        syncModule!.InvokeVoid("downloadFromUri", [fileName, uri]);
    }

    public void Download(byte[] bytes, string fileName = "")
    {
        syncModule!.InvokeVoid("downloadBytes", [fileName, bytes]);
    }

    public void Download(Stream stream, string fileName = "", bool leaveOpen = true)
    {
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        if (!leaveOpen)
            stream.Dispose();
        this.Download(memory.ToArray(), fileName);
    }
}
