namespace CaravelTemplate.Api.Shared.Messaging;

public record MassTransitOptions(string Host, ushort Port, string VirtualHost, string Username, string Password)
{
    
}