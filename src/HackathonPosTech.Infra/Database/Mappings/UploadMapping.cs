using HackathonPosTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackathonPosTech.Infra.Database.Mappings;
public class UploadMapping : IEntityTypeConfiguration<Upload>
{
    public void Configure(EntityTypeBuilder<Upload> builder)
    {
        builder.ToTable("Upload");

        builder.HasKey(x => x.Id);

        builder.Property(p => p.Email)
            .IsRequired();

        builder.Property(p => p.Nome)
            .IsRequired();

        builder.Property(p => p.CaminhoUpload)
            .IsRequired();

        builder.Property(p => p.CaminhoZip)
            .IsRequired();
    }
}