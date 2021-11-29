using Newtonsoft.Json;

namespace OneNet.PubSub.Server.DTOs
{
    public class FindTopicRequest
    {
        [JsonProperty("name")] public string Name { get; set; }
    }
}