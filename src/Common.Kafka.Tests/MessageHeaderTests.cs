using Xunit;

namespace Common.Kafka.Tests
{
    public class MessageHeaderTests
    {
        [Fact]
        public void ConstructorReturnsMessageHeader()
        {
            const string type = "Sample.Type.Message";
            const string topic = "sample-topic";

            var sut = new MessageHeader(type, topic);

            Assert.Equal(type, sut.Type);
        }

        [Fact]
        public void GetTopicReturnsTopic()
        {
            const string type = "Sample.Type.Message";
            const string topic = "sample-topic";

            var sut = new MessageHeader(type, topic);

            Assert.Equal(topic, sut.GetTopic());
        }
    }
}