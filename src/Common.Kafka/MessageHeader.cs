namespace Common.Kafka
{
    public class MessageHeader
    {
        private readonly string _topic;

        public MessageHeader(string type, string topic)
        {
            Type = type;
            _topic = topic;
        }

        public string Type { get; }

        public string GetTopic()
        {
            return _topic;
        }
    }
}