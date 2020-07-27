using MediatR;

namespace Common.Kafka
{
    public interface IMessage : INotification
    {
        string Key { get; }
    }
}