namespace Domain.Models;

public class RabbitMqConfig
{
    public int Port { get; set; }
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string VirtualHost { get; set; }
    public string Password { get; set; }
}