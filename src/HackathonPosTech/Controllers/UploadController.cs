using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HackathonPosTech.App.Data;
using HackathonPosTech.App.Models;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Azure.Storage.Blobs;

namespace HackathonPosTech.Controllers
{
    public class UploadController : Controller
    {
        private readonly UploadContext _context;
        private readonly IConfiguration _configuration;

        public UploadController(UploadContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Upload
        public async Task<IActionResult> Index()
        {
            //return _context.UploadViewModel != null ?
            //            View(await _context.UploadViewModel.ToListAsync()) :
            //            Problem("Entity set 'UploadContext.UploadViewModel'  is null.");
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
        public async Task<IActionResult> Create(UploadViewModel uploadViewModel, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                uploadViewModel.Id = Guid.NewGuid();
                _context.Add(uploadViewModel);
                await _context.SaveChangesAsync();

                SalvarArquivoLocal(files);

                return RedirectToAction(nameof(Index));
            }
            return View(uploadViewModel);
        }

        private void SalvarArquivoLocal(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        formFile.CopyToAsync(stream);
                    }
                }
            }
        }

        public string UploadImagens(string base64Image)
        {
            var BlobStorage = _configuration.GetSection("BlobStorage").Value;

            // Gera um nome randomico para imagem
            var fileName = Guid.NewGuid().ToString() + ".jpg";

            // Limpa o hash enviado
            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(base64Image, "");

            // Gera um array de Bytes
            byte[] imageBytes = Convert.FromBase64String(data);

            // Define o BLOB no qual a imagem será armazenada
            var blobClient = new BlobClient(BlobStorage, "dados", fileName);

            // Envia a imagem
            using (var stream = new MemoryStream(imageBytes))
            {
                blobClient.Upload(stream);
            }

            // Retorna a URL da imagem
            var blobUri = blobClient.Uri.AbsoluteUri;

            /*SalvarImagem(blobUri, fileName);*/

            return blobUri;
        }

        private bool UploadViewModelExists(Guid id)
        {
            //return (_context.UploadViewModel?.Any(e => e.Id == id)).GetValueOrDefault();
            return true;
        }
    }
}
