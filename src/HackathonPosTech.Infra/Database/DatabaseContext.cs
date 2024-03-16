using HackathonPosTech.Domain.Entities;
using HackathonPosTech.Infra.Database.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HackathonPosTech.Infra.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Upload> Uploads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new UploadMapping().Configure(modelBuilder.Entity<Upload>());

        base.OnModelCreating(modelBuilder);
    }
}
