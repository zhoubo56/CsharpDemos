﻿using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMqTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Send");

            var factory = new ConnectionFactory() { HostName = "192.168.132.105", UserName = "admin", Password = "admin" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent {0}", message);
                
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
