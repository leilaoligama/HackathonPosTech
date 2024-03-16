using HackathonPosTech.Domain.Dtos;

namespace HackathonPosTech.Domain.Interfaces;

public interface IEmailSender
{
    void NewsEmailSender(EmailMessageDto newsMessageDto);
}