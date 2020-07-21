using Common.Kafka;

namespace ProducerWorker.Messages
{
    public class SampleMessage : IMessage
    {
        private const string Topic = "sample-messages";
        private static readonly string Type = typeof(SampleMessage).FullName;

        public SampleMessage(string key, string someProperty)
        {
            Key = key;
            SomeProperty = someProperty;
        }

        public string SomeProperty { get; }

        public MessageHeader Header { get; } = new MessageHeader(Type, Topic);

        public string Key { get; }
    }
}