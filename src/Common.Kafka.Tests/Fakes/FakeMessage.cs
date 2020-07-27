namespace Common.Kafka.Tests.Fakes
{
    [MessageTopic("fake-messages")]
    public class FakeMessage : IMessage
    {
        public FakeMessage(string key, string someProperty)
        {
            Key = key;
            SomeProperty = someProperty;
        }

        public string Key { get; set; }

        public string SomeProperty { get; set; }
    }
}