using Common.Kafka;

namespace ProducerWorker.Messages
{
    [MessageTopic("sample-messages")]
    public class SampleMessage : IMessage
    {
        private static readonly string Type = typeof(SampleMessage).FullName;

        public SampleMessage(string key, string someProperty)
        {
            Key = key;
            SomeProperty = someProperty;
        }

        public string SomeProperty { get; }

        public MessageHeader Header { get; } = new MessageHeader(Type);

        public string Key { get; }
    }
}