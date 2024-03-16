using HackathonPosTech.Domain.Dtos;

namespace HackathonPosTech.Domain.Interfaces;

public interface IMessagePublisher
{
    Task QueueUploadMessageAsync(UploadMessageDto content);
    Task QueueEmailMessageAsync(EmailMessageDto content);
}