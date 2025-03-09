using Azure.Storage.Queues;

namespace App.Infrastructure.Storage
{
    public class QueueClientFactory
    {
        private readonly string _connectionString;

        public QueueClientFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public QueueClient CreateClient(string queueName)
        {
            return new QueueClient(_connectionString, queueName);
        }
    }
}
