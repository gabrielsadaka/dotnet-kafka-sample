using Common.Kafka;

namespace ProducerWorker.Messages
{
    [MessageTopic("other-sample-messages")]
    public class OtherSampleMessage : IMessage
    {
        private static readonly string Type = typeof(OtherSampleMessage).AssemblyQualifiedName;

        public OtherSampleMessage(string key, string someOtherProperty)
        {
            Key = key;
            SomeOtherProperty = someOtherProperty;
        }

        public string SomeOtherProperty { get; }

        public MessageHeader Header { get; } = new MessageHeader(Type);

        public string Key { get; }
    }
}