using Common.Kafka;

namespace ProducerWorker.Messages
{
    [MessageTopic("other-sample-messages")]
    public class OtherSampleMessage : IMessage
    {
        public OtherSampleMessage(string key, string someOtherProperty)
        {
            Key = key;
            SomeOtherProperty = someOtherProperty;
        }

        public string SomeOtherProperty { get; }

        public string Key { get; }
    }
}