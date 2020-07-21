namespace Common.Kafka
{
    public class MessageHeader
    {
        public MessageHeader(string type)
        {
            Type = type;
        }

        public string Type { get; }
    }
}