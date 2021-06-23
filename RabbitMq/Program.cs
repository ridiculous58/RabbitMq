using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://localhost:5672/");

            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.QueueDeclare("queue", false, false, false, null);
            channel.ExchangeDeclare("direct", ExchangeType.Direct);
            channel.QueueBind("queue", "direct", "queRoute");
            while (true)
            {
                CreatePublisher(channel);
            }

        }

        public static void CreatePublisher(IModel channel)
        {
            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("Hello, world!");
            channel.BasicPublish("direct", "queRoute", null, messageBodyBytes);
        }
       
    }
}
