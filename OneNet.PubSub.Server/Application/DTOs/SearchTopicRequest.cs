using Newtonsoft.Json;

namespace OneNet.PubSub.Server.Application.DTOs
{
    public class SearchTopicRequest
    {
        [JsonProperty("name")] public string Name { get; set; }
    }
}