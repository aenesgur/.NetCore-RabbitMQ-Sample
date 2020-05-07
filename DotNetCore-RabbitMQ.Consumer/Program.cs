using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace DotNetCore_RabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://bjeecnej:Ur4MM9S_ZZVpCQZ9rgx1Emtd_E8xyGDI@woodpecker.rmq.cloudamqp.com/bjeecnej");

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var consumer = new EventingBasicConsumer(channel); 

                    channel.BasicConsume("UserInfo-queue", false, consumer);

                    Console.WriteLine("Listening to queue");

                    consumer.Received += (model, ea) => 
                    {
                        string message = Encoding.UTF8.GetString(ea.Body);

                        var deseriliazedMessage = JsonConvert.DeserializeObject<PersonalInfo>(message);

                        Console.WriteLine($"Message revieced --> {deseriliazedMessage.Name} - {deseriliazedMessage.Surname}");

                        channel.BasicAck(ea.DeliveryTag, false);
                    };
                    Console.WriteLine("Press enter the stop listening the queue...");
                    Console.ReadLine();

                }
            }
        }
    }
}
