# FileDownloadBlazor

Download file in blazor apps. Supports asynchronously and synchronously downloading from uris, byte arrays, and streams.

## Notes

1. `ISyncFileDownloader` is not registered as a service, use `IFileDownloader.Sync` instead.
2. `IFileDownloader.Sync` is only available for Blazor WebAssembly.
3. `IFileDownloader.Sync` may throw exceptions if the JavaScript module has not been imported, since the modules can only be imported asynchronously.
    - But you don't have to worry about that, since it's not multithreaded.
    - If you do want to ensure it has been imported, use `IFileDownloader.CheckJavaScriptModuleAsync`.

## Usage

In `Program.cs`:

```csharp
using FileDownloadBlazor.Extensions;

builder.Services.AddFileDownloadBlazor();
```

In your page:

```razor
@using FileDownloadBlazor
@inject IFileDownloader Downloader

<button @onclick="TestAsyncUri">TestAsyncUri</button>
<button @onclick="TestAsyncBytes">TestAsyncBytes</button>
<button @onclick="TestAsyncStream">TestAsyncStream</button>
<button @onclick="TestSyncUri">TestSyncUri</button>
<button @onclick="TestSyncBytes">TestSyncBytes</button>
<button @onclick="TestSyncStream">TestSyncStream</button>

@code
{
    private async Task TestAsyncUri()
    {
        await Downloader.DownloadAsync(
            "./", 
            "uri-async.html");
    }

    private async Task TestAsyncBytes()
    {
        await Downloader.DownloadAsync(
            [72, 101, 108, 108, 111, 44, 32, 119, 111, 114, 108, 100, 33],
            "bytes-async.txt");
    }

    private async Task TestAsyncStream()
    {
        using var stream = new MemoryStream();
        stream.WriteByte(72);
        stream.WriteByte(101);
        stream.Position = 1;
        await Downloader.DownloadAsync(
            stream,
            "stream-async.txt");
    }

    private void TestSyncUri()
    {
        Downloader.Sync.Download(
            "./",
            "uri-sync.html");
    }

    private void TestSyncBytes()
    {
        Downloader.Sync.Download(
            [72, 101, 108, 108, 111, 44, 32, 119, 111, 114, 108, 100, 33],
            "bytes-sync.txt");
    }

    private void TestSyncStream()
    {
        using var stream = new MemoryStream();
        stream.WriteByte(72);
        stream.WriteByte(101);
        stream.Position = 1;
        Downloader.Sync.Download(
            stream,
            "stream-sync.txt");
    }
}
```
