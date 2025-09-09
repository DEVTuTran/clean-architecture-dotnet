using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using CleanArchitecture.Application.Interfaces.Storage;

namespace CleanArchitecture.Infrastructure.Services.AWS.Storage;

public class S3 : IS3
{
    private const string ContentTypeExcel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    private readonly AwsS3Options _configuration;
    private readonly IAmazonS3 _amazonS3;

    public S3(IOptions<AwsS3Options> awsConfig, IAmazonS3 amazonS3)
    {
        _configuration = awsConfig.Value;
        _amazonS3 = amazonS3;
    }

    public string GeneratePreSignedURLUpload(string filename, bool isTemporary = false)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = GetBucket(isTemporary),
            Key = filename,
            Expires = DateTime.UtcNow.AddHours(1),
            Verb = HttpVerb.PUT,
            ContentType = ContentTypeExcel
        };
        return _amazonS3.GetPreSignedURL(request);
    }

    public async Task UploadAsync(Stream stream, string filename)
    {
        var request = new PutObjectRequest
        {
            BucketName = _configuration.Bucket,
            Key = filename,
            InputStream = stream,
            ContentType = ContentTypeExcel,
            CannedACL = S3CannedACL.Private
        };

        await _amazonS3.PutObjectAsync(request);
    }

    public async Task<Stream> GetFileStreamAsync(string filename = "", bool isTemporary = false)
    {
        var request = new GetObjectRequest
        {
            BucketName = GetBucket(isTemporary),
            Key = filename
        };
        var responseObject = await _amazonS3.GetObjectAsync(request);
        return responseObject.ResponseStream;
    }

    public string GetFilePreSignedURL(string key, string filename)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _configuration.Bucket,
            Key = key,
            Expires = DateTime.UtcNow.AddHours(1),
            Verb = HttpVerb.GET,
            ResponseHeaderOverrides = new ResponseHeaderOverrides
            {
                ContentDisposition = $"attachment; filename={UrlEncoder.Default.Encode(filename)}"
            }
        };
        return _amazonS3.GetPreSignedURL(request);
    }

    public async Task DeleteAsync(string key)
    {
        if (!string.IsNullOrEmpty(key))
        {
            await _amazonS3.DeleteObjectAsync(_configuration.Bucket, key);
        }
    }

    public async Task DeleteBulkAsync(IEnumerable<string> keys)
    {
        var filteredKeys = keys?.Where(x => !string.IsNullOrEmpty(x)).ToList() ?? new List<string>();
        if (filteredKeys.Count == 0)
        {
            return;
        }

        var deleteObjectsRequest = new DeleteObjectsRequest
        {
            BucketName = _configuration.Bucket,
            Objects = filteredKeys.Select(x => new KeyVersion { Key = x }).ToList()
        };

        await _amazonS3.DeleteObjectsAsync(deleteObjectsRequest);
    }

    public async Task<string> TemporaryPutS3Async(Stream stream)
    {
        var filename = $"{Guid.NewGuid()}.xlsx";
        var request = new PutObjectRequest
        {
            BucketName = _configuration.TemporaryBucket,
            Key = filename,
            InputStream = stream,
            ContentType = ContentTypeExcel,
            CannedACL = S3CannedACL.Private
        };
        await _amazonS3.PutObjectAsync(request);
        return filename;
    }

    public string TemporaryS3FileUrl(string key, string filename)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _configuration.TemporaryBucket,
            Key = key,
            Expires = DateTime.UtcNow.AddHours(24),
            Verb = HttpVerb.GET,
            ResponseHeaderOverrides = new ResponseHeaderOverrides
            {
                ContentDisposition = $"attachment; filename={UrlEncoder.Default.Encode(filename)}"
            }
        };
        return _amazonS3.GetPreSignedURL(request);
    }

    private string GetBucket(bool isTemporary)
    {
        var bucket = isTemporary ? _configuration.TemporaryBucket : _configuration.Bucket;
        if (string.IsNullOrWhiteSpace(bucket))
        {
            throw new InvalidOperationException("AWS S3 bucket is not configured.");
        }
        return bucket;
    }
}


