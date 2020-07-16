using ProducerWorker.Infrastructure.Messaging;

namespace ProducerWorker.Messages
{
    public class SampleMessage : IMessage
    {
        public SampleMessage(string key, string someProperty)
        {
            Key = key;
            SomeProperty = someProperty;
        }

        public string SomeProperty { get; }

        public string GetTopic()
        {
            return "sample-messages";
        }

        public string Key { get; }
    }
}