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

        public string Key { get; }

        public string AnotherProperty { get; }
    }
}