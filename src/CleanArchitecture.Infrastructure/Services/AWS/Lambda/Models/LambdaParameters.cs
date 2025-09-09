using Newtonsoft.Json;

namespace CleanArchitecture.Infrastructure.Services.AWS.Lambda.Models;

public class LambdaParameters
{
    [JsonProperty("queryStringParameters", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    public virtual QueryStringParameters QueryStringParameters { get; set; } = new();
}

public partial class QueryStringParameters
{
    [JsonProperty("url", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    public virtual Uri? Url { get; set; }

    [JsonProperty("key", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    public virtual string Key { get; set; } = string.Empty;
}

public static partial class LambdaParametersExtension
{
    public static string ToJson(this LambdaParameters self)
    {
        return JsonConvert.SerializeObject(self);
    }
}
