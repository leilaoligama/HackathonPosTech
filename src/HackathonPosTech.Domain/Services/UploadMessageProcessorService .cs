using Azure.Storage.Blobs;
using FFMpegCore;
using HackathonPosTech.Domain.Dtos;
using HackathonPosTech.Domain.Entities;
using HackathonPosTech.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace HackathonPosTech.Domain.Services;

public class UploadMessageProcessorService : IMessageProcessorService<UploadMessageDto>
{
    private readonly IBaseRepository<Upload> _repository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IConfiguration _configuration;

    public UploadMessageProcessorService(IBaseRepository<Upload> repository, IMessagePublisher messagePublisher, IConfiguration configuration)
    {
        _repository = repository;
        _messagePublisher = messagePublisher;
        _configuration = configuration;
    }

    public async Task ProcessMessage(UploadMessageDto? dto)
    {
        var upload = await _repository.FindAsync(dto.IdUpload);

        var videoInfo = FFProbe.Analyse(upload.CaminhoUpload);
        var duration = videoInfo.Duration;
        
        var outputFolder = Directory.GetCurrentDirectory();
        var imagesFolder = Path.Combine(outputFolder, "ImageFiles");
        
        Directory.CreateDirectory(imagesFolder);

        var interval = TimeSpan.FromSeconds(20);

        for (var currentTime = TimeSpan.Zero; currentTime < duration; currentTime += interval)
        {
            Console.WriteLine($"Processando frame: {currentTime}");

            var outputPath = Path.Combine(imagesFolder, $"frame_at_{currentTime.TotalSeconds}.jpg");
            FFMpeg.Snapshot(upload.CaminhoUpload, imagesFolder, new Size(1920, 1080), currentTime);
        }

        var fileName = $"{upload.Id}.zip";
        var destinationZipFilePath = Path.Combine(imagesFolder, fileName);

        ZipFile.CreateFromDirectory(imagesFolder, destinationZipFilePath);

        var zipBytes = File.ReadAllBytes(destinationZipFilePath);

        var caminhoZip = UploadZip(zipBytes, fileName);

        upload.CaminhoZip = caminhoZip;
        _repository.Update(upload);

        await _messagePublisher.QueueEmailMessageAsync(new EmailMessageDto
        {
            CaminhoArquivoZip = upload.CaminhoZip,
            Email = upload.Email,
            Nome = upload.Nome,
        });
    }

    public string UploadZip(byte[] zipFileBytes, string fileName)
    {
        var BlobStorage = _configuration.GetSection("BlobStorage").Value;

        var blobClient = new BlobClient(BlobStorage, "zip", fileName);

        using (var stream = new MemoryStream(zipFileBytes))
        {
            blobClient.Upload(stream);
        }

        var blobUri = blobClient.Uri.AbsoluteUri;

        return blobUri;
    }
}