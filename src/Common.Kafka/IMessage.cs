using MediatR;

namespace Common.Kafka
{
    public interface IMessage : INotification
    {
        MessageHeader Header { get; }
        string Key { get; }
    }
}