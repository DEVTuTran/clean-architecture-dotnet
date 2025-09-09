namespace CleanArchitecture.Infrastructure.Services.AWS.Storage;

public sealed class AwsS3Options
{
    public string AccessKey { get; set; } = string.Empty;
    public string SecretAccessKey { get; set; } = string.Empty;
    public string Bucket { get; set; } = string.Empty;
    public string TemporaryBucket { get; set; } = string.Empty;
    public string BackupBucket { get; set; } = string.Empty;
    public string Region { get; set; } = "us-east-1"; // Default region
}


