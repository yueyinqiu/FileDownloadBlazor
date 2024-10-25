
namespace FileDownloadBlazor;

public interface IFileDownloaderSync
{
    void Dispose();
    void Download(byte[] bytes, string fileName = "");
    void Download(Stream stream, string fileName = "", bool leaveOpen = true);
    void Download(string uri, string fileName = "");
    Task DownloadAsync(Stream stream, string fileName = "", bool leaveOpen = true, CancellationToken cancellationToken = default);
}