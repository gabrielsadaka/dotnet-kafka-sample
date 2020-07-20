namespace Common.Kafka
{
    public interface IMessage
    {
        string Key { get; }
        string GetTopic();
    }
}