using Xunit;

namespace Common.Kafka.Tests
{
    public class MessageHeaderTests
    {
        [Fact]
        public void ConstructorReturnsMessageHeader()
        {
            const string type = "Sample.Type.Message";

            var sut = new MessageHeader(type);

            Assert.Equal(type, sut.Type);
        }
    }
}