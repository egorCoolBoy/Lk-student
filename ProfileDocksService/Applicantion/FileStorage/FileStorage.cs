using ProfileDocksService.Applicantion.Options;

namespace ProfileDocksService.Applicantion.FileStorage;

using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

public class FileStorage : IFileStorage
{
    private readonly MinioClient _minio;
    private readonly MinioOptions _options;

    public FileStorage(IOptions<MinioOptions> options)
    {
        _options = options.Value;

        _minio = (MinioClient)new MinioClient()
            .WithEndpoint(_options.Endpoint)
            .WithCredentials(
                _options.AccessKey,
                _options.SecretKey)
            .Build();
    }

    public async Task<string> UploadAsync(
        Stream stream,
        string fileName,
        string contentType)
    {
        var extension = Path.GetExtension(fileName);

        var storageKey =
            $"documents/{Guid.NewGuid()}{extension}";

        await _minio.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(storageKey)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType));

        return storageKey;
    }

    public async Task<Stream> DownloadAsync(
        string storageKey)
    {
        var memory = new MemoryStream();

        await _minio.GetObjectAsync(
            new GetObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(storageKey)
                .WithCallbackStream(stream =>
                {
                    stream.CopyTo(memory);
                }));

        memory.Position = 0;

        return memory;
    }

    public async Task DeleteAsync(
        string storageKey)
    {
        await _minio.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(storageKey));
    }
}