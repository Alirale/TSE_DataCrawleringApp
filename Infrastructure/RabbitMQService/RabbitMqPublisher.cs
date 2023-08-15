using RabbitMQ.Client;
using System.Text;
using Domain.Models;

namespace Infrastructure.RabbitMQService
{
    public class RabbitMqPublisher
    {
        private readonly RabbitMqConfig _config;

        public RabbitMqPublisher(RabbitMqConfig config)
        {
            _config = config;
        }

        public void PublishMessage(string exchangeName, string message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.HostName,
                Port = _config.Port,
                UserName = _config.UserName,
                Password = _config.Password,
                VirtualHost = _config.VirtualHost
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
            var body = Encoding.UTF8.GetBytes(message);

            channel.QueueDeclare(queue: exchangeName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicPublish(exchange: "", routingKey: exchangeName, basicProperties: null, body: body);
        }
    }
}
