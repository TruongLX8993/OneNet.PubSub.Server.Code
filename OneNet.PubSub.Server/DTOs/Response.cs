using Newtonsoft.Json;

namespace OneNet.PubSub.Server.DTOs
{
    public class Response
    {
        [JsonProperty("status")] public int Status { get; set; }
        [JsonProperty("data")] public object Data { get; set; }
        [JsonProperty("message")] public string Message { get; set; }
    }
}