using Microsoft.JSInterop;

namespace FileDownloadBlazor;

internal sealed class FileDownloaderSync(IJSInProcessRuntime jsRuntime) : IDisposable, IFileDownloaderSync
{
    private readonly IJSInProcessObjectReference module =
        jsRuntime.Invoke<IJSInProcessObjectReference>(
            "import",
            "./_content/FileDownloadBlazor/downloader.js");
    public void Download(string uri, string fileName = "")
    {
        module.InvokeVoid("downloadFromUri", [fileName, uri]);
    }
    public void Download(byte[] bytes, string fileName = "")
    {
        module.InvokeVoid("downloadBytes", [fileName, bytes]);
    }
    public void Download(Stream stream, string fileName = "", bool leaveOpen = true)
    {
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        if (!leaveOpen)
            stream.Dispose();
        this.Download(memory.ToArray(), fileName);
    }
    public async Task DownloadAsync(
        Stream stream, string fileName = "", bool leaveOpen = true,
        CancellationToken cancellationToken = default)
    {
        using var reference = new DotNetStreamReference(stream, leaveOpen);
        await module.InvokeVoidAsync("downloadStream", cancellationToken, [fileName, reference]);
    }
    public void Dispose()
    {
        module.Dispose();
    }
}
