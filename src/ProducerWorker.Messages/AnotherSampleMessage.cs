using Common.Kafka;

namespace ProducerWorker.Messages
{
    [MessageTopic("sample-messages")]
    public class AnotherSampleMessage : IMessage
    {
        public AnotherSampleMessage(string key, string anotherProperty)
        {
            Key = key;
            AnotherProperty = anotherProperty;
        }

        public string AnotherProperty { get; }

        public MessageHeader Header { get; } = new MessageHeader();

        public string Key { get; }
    }
}