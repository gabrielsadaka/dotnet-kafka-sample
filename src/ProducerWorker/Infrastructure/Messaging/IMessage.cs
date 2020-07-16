namespace ProducerWorker.Infrastructure.Messaging
{
    public interface IMessage
    {
        string Key { get; }
        string GetTopic();
    }
}