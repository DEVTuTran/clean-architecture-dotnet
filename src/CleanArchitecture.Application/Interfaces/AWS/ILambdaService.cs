namespace CleanArchitecture.Application.Interfaces.AWS;

using System.Threading.Tasks;

public interface ILambdaService
{
    Task<LambdaResponse> InvokeHtmlToPdfFunctionAsync(string url);
    Task<string> InvokeOfficeToPdfFunctionAsync(byte[] pptx, string filename);
    Task<string> PowerpointTemporaryAsync(byte[] pptx, string filename);
}

public class LambdaResponse
{
    public long? StatusCode { get; set; }
    public string Body { get; set; } = string.Empty;
}


