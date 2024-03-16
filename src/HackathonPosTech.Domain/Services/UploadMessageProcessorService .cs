using HackathonPosTech.Domain.Dtos;
using HackathonPosTech.Domain.Entities;
using HackathonPosTech.Domain.Interfaces;

namespace HackathonPosTech.Domain.Services;

public class UploadMessageProcessorService : IMessageProcessorService<UploadMessageDto>
{
    private readonly IBaseRepository<Upload> _repository;
    private readonly IMessagePublisher _messagePublisher;

    public UploadMessageProcessorService(IBaseRepository<Upload> repository, IMessagePublisher messagePublisher)
    {
        _repository = repository;
        _messagePublisher = messagePublisher;
    }

    public async Task ProcessMessage(UploadMessageDto? dto)
    {
        // processar o video
        // salvar as imagens
        // gerar zip
        // upload do zip para o blob
        // atualizar a tabela upload com o caminho do zip
        // publicar mensagem para envio do email com o caminho do zip
    }
}