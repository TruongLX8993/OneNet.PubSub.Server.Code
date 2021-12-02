using System;

namespace OneNet.PubSub.Server.Application.Domains
{
    public class Topic
    {
        public string Id { get; }
        public string Name { get; set; }
        public TopicConfig TopicConfig { get; set; }
        public Connection OwnerConnection { get; set; }
        public string OwnerUserName => OwnerConnection.UserName;
        public string OwnerConnectionId => OwnerConnection.Id;
        public DateTime? CreatedTime { get; }

        public Topic()
        {
            CreatedTime = new DateTime();
            Id = new Guid().ToString();
        }

        public bool CanUpdateOwnerConnection(Connection connection)
        {
            return connection.Id == OwnerConnectionId ||
                   (TopicConfig.IsUpdateOwnerConnection && OwnerUserName == connection.UserName);
        }

        public void UpdateConnectionOwner(Connection connection, bool skippedCheck = false)
        {
            if (!skippedCheck && !CanUpdateOwnerConnection(connection))
            {
                throw new Exception("Can not update owner connection ");
            }

            OwnerConnection = connection;
        }

        public bool IsOwnerConnection(Connection currentConnection)
        {
            return OwnerConnectionId == currentConnection.Id;
        }

        public bool IsAbortWhenOwnerDisconnect()
        {
            return !TopicConfig.IsKeepTopicWhenOwnerDisconnect;
        }
    }
}