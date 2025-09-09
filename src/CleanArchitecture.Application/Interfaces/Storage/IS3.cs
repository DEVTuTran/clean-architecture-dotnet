using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces.Storage;

public interface IS3
{
    Task DeleteAsync(string key);
    Task DeleteBulkAsync(IEnumerable<string> keys);
    string GeneratePreSignedURLUpload(string filename, bool isTemporary = false);
    string GetFilePreSignedURL(string key, string filename);
    Task<Stream> GetFileStreamAsync(string filename = "", bool isTemporary = false);
    Task<string> TemporaryPutS3Async(Stream stream);
    string TemporaryS3FileUrl(string key, string filename);
    Task UploadAsync(Stream stream, string filename);
}


