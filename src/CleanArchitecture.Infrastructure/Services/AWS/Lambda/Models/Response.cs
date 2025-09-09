using Newtonsoft.Json;

namespace CleanArchitecture.Infrastructure.Services.AWS.Lambda.Models;

public class Response
{
    [JsonProperty("statusCode", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    public virtual long? StatusCode { get; set; }

    [JsonProperty("body", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    public virtual string Body { get; set; } = string.Empty;
}

public static class ResponseExtension
{
    public static Response FromJson(string json) => JsonConvert.DeserializeObject<Response>(json) ?? new Response();
}
