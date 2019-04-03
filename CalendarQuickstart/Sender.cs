using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Xml;
using CalendarQuickstart.Logic;

namespace CalendarQuickstart
{
    class Sender
    {
            public void SendMessage(string doc)
            {
                var factory = new ConnectionFactory() { HostName = "localhost", UserName = "", Password = "", Port = 5672 };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(doc);
                    Console.WriteLine(body);
                    channel.BasicPublish(exchange: "RabbitMQ",
                        routingKey: "",
                        basicProperties: null,
                        body: body);
                }
            }
        }
    }

