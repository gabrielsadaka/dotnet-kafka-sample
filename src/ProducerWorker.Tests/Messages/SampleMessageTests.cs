using ProducerWorker.Messages;
using Xunit;

namespace ProducerWorker.Tests.Messages
{
    public class SampleMessageTests
    {
        [Fact]
        public void ConstructorReturnsSampleMessage()
        {
            const string key = "some-key-1";
            const string someProperty = "some-property-value";

            var sut = new SampleMessage(key, someProperty);

            Assert.Equal(key, sut.Key);
            Assert.Equal(someProperty, sut.SomeProperty);
        }

        [Fact]
        public void HeaderGetTopicReturnsCorrectTopic()
        {
            const string topic = "sample-messages";

            var sut = new SampleMessage("some-key-1", "some-property-value");

            Assert.Equal(topic, sut.Header.GetTopic());
        }
    }
}