using Common.Kafka;

namespace ProducerWorker.Messages
{
    [MessageTopic("sample-messages")]
    public class AnotherSampleMessage : IMessage
    {
        private static readonly string Type = typeof(AnotherSampleMessage).AssemblyQualifiedName;

        public AnotherSampleMessage(string key, string anotherProperty)
        {
            Key = key;
            AnotherProperty = anotherProperty;
        }

        public string AnotherProperty { get; }

        public MessageHeader Header { get; } = new MessageHeader(Type);

        public string Key { get; }
    }
}