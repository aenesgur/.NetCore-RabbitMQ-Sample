using DotNetCore_RabbitMQ.UI.Producer.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore_RabbitMQ.UI.Producer.Helper
{
    
    public class RabbitMqHelper
    {
        private readonly IConfiguration _configuration;
        public RabbitMqHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConnectionRabbitMq(PersonalInfo personalInfo)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri(_configuration["ConnectionStrings:RabbitMqCloudString"]);
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "UserInfo-exchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                    channel.QueueDeclare(queue: "UserInfo-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    channel.QueueBind(queue: "UserInfo-queue", exchange: "UserInfo-exchange", routingKey: "BasicInfo");

                    var serializeMessage = JsonConvert.SerializeObject(personalInfo);
                    var byteMessage = Encoding.UTF8.GetBytes(serializeMessage);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "UserInfo-exchange", routingKey: "BasicInfo", basicProperties: properties, body: byteMessage);
                }
            }
        }
    }
}
