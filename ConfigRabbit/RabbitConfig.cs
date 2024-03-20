using RabbitMQ.Client;
using System;

public sealed class RabbitMQConfig
{
    private static readonly Lazy<RabbitMQConfig> _instance;

    static RabbitMQConfig()
    {
        _instance = new Lazy<RabbitMQConfig>(new RabbitMQConfig());
    }

    private readonly ConnectionFactory _factory;

    private RabbitMQConfig()
    {
        _factory = new ConnectionFactory
        {
            Uri = new Uri("amqp://guest:guest@localhost:5672")
        };
    }

    public static RabbitMQConfig Instance => _instance.Value;

    public IConnection CreateConnection()
    {
        return _factory.CreateConnection();
    }

    public IModel CreateModel(IConnection connection)
    {
        return connection.CreateModel();
    }
}