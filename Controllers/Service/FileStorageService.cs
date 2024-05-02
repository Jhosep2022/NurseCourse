using System;
using System.IO;
using System.Threading.Tasks;
using Minio;
using Minio.Exceptions;
using Microsoft.Extensions.Configuration;

public class FileStorageService
{
    private readonly MinioClient _minioClient;
    private readonly string _bucketName;
    private readonly bool _useSSL;
    private readonly string _endpoint;

    public FileStorageService(IConfiguration configuration)
    {
        // Trata de analizar la configuración de SSL y usa un valor predeterminado si no está presente.
        _useSSL = configuration.GetValue<bool>("MinioConfig:UseSSL", false);
        _endpoint = configuration["MinioConfig:Endpoint"];

        var accessKey = configuration["MinioConfig:AccessKey"];
        var secretKey = configuration["MinioConfig:SecretKey"];
        
        _minioClient = new MinioClient(_endpoint, accessKey, secretKey);

        if (_useSSL)
        {
            _minioClient.WithSSL();
        }

        _bucketName = configuration["MinioConfig:BucketName"];
        EnsureBucketExistsAsync().Wait();  // Considera manejar esto de manera asincrónica si es posible
    }

    private async Task EnsureBucketExistsAsync()
    {
        try
        {
            bool found = await _minioClient.BucketExistsAsync(_bucketName);
            if (!found)
            {
                await _minioClient.MakeBucketAsync(_bucketName);
            }
        }
        catch (MinioException e)
        {
            Console.WriteLine($"Error checking/creating bucket: {e.Message}");
            throw;
        }
    }

    public async Task<string> UploadFileAsync(Stream data, string fileName)
    {
        try
        {
            await _minioClient.PutObjectAsync(_bucketName, fileName, data, data.Length);
            return GetFileUrl(fileName);
        }
        catch (MinioException e)
        {
            Console.WriteLine($"Error uploading file to MinIO: {e.Message}");
            throw;
        }
    }

    public string GetFileUrl(string fileName)
    {
        string protocol = _useSSL ? "https" : "http";
        return $"{protocol}://{_endpoint}/{_bucketName}/{fileName}";
    }
}