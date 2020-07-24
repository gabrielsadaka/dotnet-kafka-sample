using Common.Kafka;

namespace ProducerWorker.Messages
{
    [MessageTopic("sample-messages")]
    public class SampleMessage : IMessage
    {
        public SampleMessage(string key, string someProperty)
        {
            Key = key;
            SomeProperty = someProperty;
        }

        public string SomeProperty { get; }

        public MessageHeader Header { get; } = new MessageHeader();

        public string Key { get; }
    }
}