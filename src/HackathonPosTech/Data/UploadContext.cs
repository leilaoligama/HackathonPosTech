using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HackathonPosTech.App.Models;

namespace HackathonPosTech.App.Data
{
    public class UploadContext : DbContext
    {
        public UploadContext (DbContextOptions<UploadContext> options)
            : base(options)
        {
        }

        public DbSet<HackathonPosTech.App.Models.UploadViewModel> UploadViewModel { get; set; } = default!;
    }
}
