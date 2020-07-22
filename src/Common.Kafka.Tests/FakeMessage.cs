namespace Common.Kafka.Tests
{
    [MessageTopic("fake-messages")]
    public class FakeMessage : IMessage
    {
        private static readonly string Type = typeof(FakeMessage).FullName;

        public FakeMessage(string key, string someProperty)
        {
            Key = key;
            SomeProperty = someProperty;
        }

        public string SomeProperty { get; set; }

        public MessageHeader Header { get; } = new MessageHeader(Type);

        public string Key { get; set; }
    }
}