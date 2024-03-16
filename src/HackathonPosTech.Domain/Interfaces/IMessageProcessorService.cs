namespace HackathonPosTech.Domain.Interfaces;

public interface IMessageProcessorService<T>
{
    Task ProcessMessage(T dto);
}