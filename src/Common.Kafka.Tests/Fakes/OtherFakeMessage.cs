namespace Common.Kafka.Tests.Fakes
{
    [MessageTopic("other-fake-messages")]
    public class OtherFakeMessage : IMessage
    {
        public OtherFakeMessage(string key, string someOtherProperty)
        {
            Key = key;
            SomeOtherProperty = someOtherProperty;
        }

        public string SomeOtherProperty { get; set; }

        public MessageHeader Header { get; } = new MessageHeader();

        public string Key { get; set; }
    }
}