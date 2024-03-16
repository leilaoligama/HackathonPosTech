namespace HackathonPosTech.Domain.Interfaces;

public interface IMessageProcessor
{
    Task StartAsync(CancellationToken cancellationToken);
    Task StopAsync(CancellationToken cancellationToken);
}