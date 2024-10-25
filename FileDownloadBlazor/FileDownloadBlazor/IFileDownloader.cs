
namespace FileDownloadBlazor;

public interface IFileDownloader
{
    Task DownloadAsync(byte[] bytes, string fileName = "", CancellationToken cancellationToken = default);
    Task DownloadAsync(Stream stream, string fileName = "", bool leaveOpen = true, CancellationToken cancellationToken = default);
    Task DownloadAsync(string uri, string fileName = "", CancellationToken cancellationToken = default);
    ISyncFileDownloader Sync { get; }
}