using Common.Kafka.Tests.Fakes;
using Xunit;

namespace Common.Kafka.Tests
{
    public class MessageNotificationTests
    {
        [Fact]
        public void ConstructorReturnsMessageNotification()
        {
            var fakeMessage = new FakeMessage("some-key", "some-property");

            var sut = new MessageNotification<FakeMessage>(fakeMessage);

            Assert.Equal(fakeMessage, sut.Message);
        }
    }
}