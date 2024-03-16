using HackathonPosTech.App.Models;
using HackathonPosTech.Domain.Dtos;
using HackathonPosTech.Domain.Entities;
using HackathonPosTech.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackathonPosTech.Controllers
{
    public class UploadController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseRepository<Upload> _repository;
        private readonly IMessagePublisher _messagePublisher;

        public UploadController(IConfiguration configuration, IBaseRepository<Upload> repository, IMessagePublisher messagePublisher)
        {
            _configuration = configuration;
            _repository = repository;
            _messagePublisher = messagePublisher;
        }

        // GET: Upload
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Upload/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Upload/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UploadViewModel uploadViewModel, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var caminhoUpload = SalvarArquivoLocal(file);

                var upload = new Upload
                {
                    Id = Guid.NewGuid(),
                    Nome = uploadViewModel.Nome,
                    Email = uploadViewModel.Email,
                    CaminhoUpload = caminhoUpload
                };
                await _repository.AddAsync(upload);

                await _messagePublisher.QueueUploadMessageAsync(new UploadMessageDto { IdUpload = upload.Id });

                return RedirectToAction(nameof(Index));
            }
            return View(uploadViewModel);
        }

        private string SalvarArquivoLocal(IFormFile file)
        {
            var filePath = Path.GetTempFileName();

            if (file.Length > 0)
            {
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyToAsync(stream);
                }
            }

            return filePath;
        }
    }
}
