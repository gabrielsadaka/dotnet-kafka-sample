using Xunit;

namespace Common.Kafka.Tests
{
    public class MessageTopicAttributeTests
    {
        [Fact]
        public void ConstructorReturnsMessageTopicAttribute()
        {
            const string topic = "sample-topic";

            var sut = new MessageTopicAttribute(topic);

            Assert.Equal(topic, sut.Topic);
        }
    }
}