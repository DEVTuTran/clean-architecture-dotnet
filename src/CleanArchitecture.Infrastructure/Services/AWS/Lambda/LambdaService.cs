using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Encodings.Web;
using CleanArchitecture.Application.Interfaces.AWS;
using CleanArchitecture.Infrastructure.Services.AWS.Lambda.Models;
using CleanArchitecture.Infrastructure.Services.AWS.Storage;

namespace CleanArchitecture.Infrastructure.Services.AWS.Lambda;

public class LambdaService : ILambdaService
{
    private readonly AwsS3Options _awsConfig;
    private readonly IAmazonS3 _amazonS3;
    private readonly IAmazonLambda _lambda;

    public LambdaService(IOptions<AwsS3Options> awsConfig, IAmazonS3 amazonS3, IAmazonLambda lambda)
    {
        _awsConfig = awsConfig.Value;
        _lambda = lambda;
        _amazonS3 = amazonS3;
    }

    public async Task<LambdaResponse> InvokeHtmlToPdfFunctionAsync(string url)
    {
        try
        {
            var model = new LambdaParameters { QueryStringParameters = new QueryStringParameters { Url = new Uri(url) } };
            var payload = model.ToJson();
            var request = new InvokeRequest
            {
                FunctionName = "HtmlToPdf",
                Payload = payload,
            };

            var response = await _lambda.InvokeAsync(request);
            MemoryStream stream = response.Payload;
            string json = Encoding.UTF8.GetString(stream.ToArray());
            var lambdaResponse = ResponseExtension.FromJson(json);

            return new LambdaResponse
            {
                StatusCode = lambdaResponse.StatusCode,
                Body = lambdaResponse.Body
            };
        }
        catch (Exception ex)
        {
            return new LambdaResponse
            {
                StatusCode = 500,
                Body = $"Error: {ex.Message}"
            };
        }
    }

    public async Task<string> InvokeOfficeToPdfFunctionAsync(byte[] pptx, string filename)
    {
        try
        {
            var model = new LambdaParameters { QueryStringParameters = new QueryStringParameters { Key = await PutS3Async(pptx) } };
            var payload = model.ToJson();
            var request = new InvokeRequest
            {
                FunctionName = "OfficeToPdfLastVersion",
                Payload = payload,
            };

            var response = await _lambda.InvokeAsync(request);
            MemoryStream stream = response.Payload;
            string json = Encoding.UTF8.GetString(stream.ToArray());
            return TemporaryS3FileUrl(ResponseExtension.FromJson(json).Body, filename);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to convert office file to PDF: {ex.Message}");
        }
    }

    public async Task<string> PowerpointTemporaryAsync(byte[] pptx, string filename)
    {
        try
        {
            var s3Key = await PutS3Async(pptx);
            return TemporaryS3FileUrl(s3Key, filename);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to upload PowerPoint file: {ex.Message}");
        }
    }

    private string TemporaryS3FileUrl(string key, string filename)
    {
        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
        {
            BucketName = _awsConfig.TemporaryBucket,
            Key = key,
            Expires = DateTime.UtcNow.AddHours(24),
            Verb = HttpVerb.GET,
            ResponseHeaderOverrides = new ResponseHeaderOverrides()
            {
                ContentDisposition = $"attachment; filename={UrlEncoder.Default.Encode(filename)}"
            }
        };
        return _amazonS3.GetPreSignedURL(request);
    }

    private async Task<string> PutS3Async(byte[] pptx)
    {
        var filename = $"{Guid.NewGuid()}.pptx";
        var request = new PutObjectRequest
        {
            BucketName = _awsConfig.TemporaryBucket,
            Key = filename,
            InputStream = new MemoryStream(pptx),
            ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            CannedACL = S3CannedACL.Private
        };
        await _amazonS3.PutObjectAsync(request);
        return filename;
    }
}


