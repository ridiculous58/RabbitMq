using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace Consumer
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
            Console.WriteLine("Consumer Çalıştı : ");
            while (true)
            {
                CreateConsumer(channel);

            }
        }

        public static void CreateConsumer(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                Console.WriteLine(System.Text.Encoding.Default.GetString(body));
                // copy or deserialise the payload
                // and process the message
                // ...
                channel.BasicAck(ea.DeliveryTag, false);
            };
            // this consumer tag identifies the subscription
            // when it has to be cancelled
            String consumerTag = channel.BasicConsume("queue", false, consumer);
        }
    }
}
