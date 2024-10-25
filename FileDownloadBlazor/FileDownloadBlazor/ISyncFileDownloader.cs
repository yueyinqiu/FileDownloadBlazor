
namespace FileDownloadBlazor;

public interface ISyncFileDownloader : IFileDownloader
{
    void Download(byte[] bytes, string fileName = "");
    void Download(Stream stream, string fileName = "", bool leaveOpen = true);
    void Download(string uri, string fileName = "");
}