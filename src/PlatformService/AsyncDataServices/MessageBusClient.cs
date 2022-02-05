using System.Text;
using System.Text.Json;
using PlatformService.DTOs;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchangeType;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _exchangeType = "trigger";

            var factory = new ConnectionFactory 
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: _exchangeType, type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("--> Connected to Message Bus");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error - {ex.Message}");
            }
        }
        public void PublishNewPlatform(PlatformPublishedDTO platformPublishedDTO)
        {
            var message = JsonSerializer.Serialize(platformPublishedDTO);

            if(_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, Sending Message");
                SendMessage(message);
            }

            if(!_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Closed, Not Sending Message");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                    exchange: _exchangeType, 
                    routingKey: "",
                    basicProperties: null,
                    body: body
                );
            Console.WriteLine($"--> Message sended: {message}");
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs args)
        {
            Console.WriteLine("--> RabbitMQ connection Shutdown");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}