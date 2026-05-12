namespace ProfileDocksService.Applicantion.FileStorage;

public interface IFileStorage
{
    Task<string> UploadAsync(
        Stream stream,
        string fileName,
        string contentType);

    Task<Stream> DownloadAsync(string storageKey);

    Task DeleteAsync(string storageKey);
}