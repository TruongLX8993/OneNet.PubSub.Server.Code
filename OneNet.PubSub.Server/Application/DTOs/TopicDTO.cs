using Newtonsoft.Json;
using OneNet.PubSub.Server.Application.Domains;

namespace OneNet.PubSub.Server.Application.DTOs
{
    public class TopicDTO
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("ownerName")] public string OwnerName { get; set; }
        [JsonProperty("ownerConnectionId")] public string OwnerConnectionId { get; set; }
        [JsonProperty("createdTime")] public string CreatedTime { get; set; }

        public TopicDTO(Topic topic)
        {
            Name = topic.Name;
            OwnerName = topic.OwnerConnection?.UserName;
            OwnerConnectionId = topic.OwnerConnection?.Id;
            CreatedTime = topic.CreatedTime.ToString();
        }
    }
}