using Newtonsoft.Json;

namespace OneNet.PubSub.Server.Apis
{
    public class ApiResponse
    {
        [JsonProperty("status")] public int Status { get; set; }
        [JsonProperty("data")] public object Data { get; set; }
        [JsonProperty("message")] public string Message { get; set; }
    }
}