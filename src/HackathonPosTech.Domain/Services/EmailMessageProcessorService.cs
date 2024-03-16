using HackathonPosTech.Domain.Dtos;
using HackathonPosTech.Domain.Entities;
using HackathonPosTech.Domain.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace HackathonPosTech.Domain.Services;

public class EmailMessageProcessorService : IMessageProcessorService<EmailMessageDto>
{
    private readonly IConfiguration _configuration;

    public EmailMessageProcessorService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ProcessMessage(EmailMessageDto dto)
    {
        try
        {
            var mailString = _configuration.GetSection("EmailService")["Address"]!;
            var mailPass = _configuration.GetSection("EmailService")["Password"];

            var message = new MailMessage();
            message.From = new MailAddress(mailString);
            message.IsBodyHtml = true;
            message.Subject = "Seu arquivo ficou pronto!";
            message.Body = $"<html><body><p>Um novo arquivo vou submetido.</p>" +
                           $"<b>A short previous:</b> <br /> <i> {dto.CaminhoArquivoZip}... </i></body></html>";

            message.Bcc.Add(dto.Email);

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(mailString, mailPass);

            smtpClient.Send(message);
        }
        catch (SmtpException ex)
        {
            throw new ApplicationException
                ("SmtpException has occured: " + ex.Message);
        }
    }
}