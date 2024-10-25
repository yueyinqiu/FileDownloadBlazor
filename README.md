# FileDownloadBlazor

## Usage

In `Program.cs`:

```csharp
builder.Services.AddFileDownloadBlazor();
```

In `_Imports.razor`:

```razor
@using FileDownloadBlazor
```

In your page:

```razor
@inject IFileDownloader Downloader

<button @onclick="DownloadHelloWorld">Hello World</button>

@code
{
    private async Task DownloadHelloWorld()
    {
        await Downloader.DownloadAsync(
            [72, 101, 108, 108, 111, 44, 32, 119, 111, 114, 108, 100, 33], 
            "HelloWorld.txt");
    }
}
```
