namespace Common.Kafka
{
    public interface IMessage
    {
        MessageHeader Header { get; }
        string Key { get; }
    }
}