namespace Common.Kafka
{
    public class KafkaOptions
    {
        public string KafkaBootstrapServers { get; set; }
        public string ConsumerGroupId { get; set; }
    }
}