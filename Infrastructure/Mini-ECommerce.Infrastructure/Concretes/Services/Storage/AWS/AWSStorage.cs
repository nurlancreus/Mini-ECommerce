using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mini_ECommerce.Application.Abstractions.Services.Storage.AWS;
using Mini_ECommerce.Infrastructure.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Concretes.Services.Storage.AWS
{
    public class AWSStorage : IAWSStorage
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public AWSStorage(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _bucketName = configuration["Storage:AWS:AWSS3:BucketName"]!;
        }

        public async Task DeleteAsync(string pathOrContainerName, string fileName)
        {
            var key = Path.Combine(pathOrContainerName, fileName).Replace("\\", "/");
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        }

        public async Task<List<string>> GetFilesAsync(string pathOrContainerName)
        {
            var files = new List<string>();
            var listObjectsRequest = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = pathOrContainerName,
                Delimiter = "/"
            };

            var listObjectsResponse = await _s3Client.ListObjectsV2Async(listObjectsRequest);
            foreach (var s3Object in listObjectsResponse.S3Objects)
            {
                files.Add(s3Object.Key);
            }

            return files;
        }

        public async Task<bool> HasFileAsync(string pathOrContainerName, string fileName)
        {
            var key = Path.Combine(pathOrContainerName, fileName).Replace("\\", "/");

            try
            {
                await _s3Client.GetObjectAsync(_bucketName, key);
                return true;
            }
            catch (AmazonS3Exception e) when (e.ErrorCode == "404" || e.ErrorCode == "403")
            {
                return false;
            }
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection formFiles)
        {
            var uploadedFiles = new List<(string fileName, string pathOrContainerName)>();

            foreach (var formFile in formFiles)
            {
                if (formFile.Length > 0)
                {
                    string fileNewName = await FileHelpers.RenameFileAsync(pathOrContainerName, formFile.FileName, HasFileAsync);
                    var key = Path.Combine(pathOrContainerName, fileNewName).Replace("\\", "/");

                    using var stream = formFile.OpenReadStream();
                    var uploadRequest = new PutObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = key,
                        InputStream = stream,
                        ContentType = formFile.ContentType
                    };

                    await _s3Client.PutObjectAsync(uploadRequest);
                    uploadedFiles.Add((fileNewName, pathOrContainerName));
                }
            }

            return uploadedFiles;
        }
    }
}
