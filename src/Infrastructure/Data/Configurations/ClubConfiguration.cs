using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace Infrastructure.Data.Configurations
{
    public class ClubConfiguration : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> b)
        {
            b.ToTable("Clubs");

            // Key
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasDefaultValueSql("NEWID()");

            // Required + lengths
            b.Property(x => x.Name)
             .IsRequired()
             .HasMaxLength(120);

            b.Property(x => x.ShortName)
             .IsRequired()
             .HasMaxLength(12);

            // Defaults
            b.Property(x => x.CreatedAt)
             .HasDefaultValueSql("SYSUTCDATETIME()")
             .IsRequired();

            // Uniqueness
            b.HasIndex(x => x.Name).IsUnique();
            b.HasIndex(x => x.ShortName).IsUnique();

        }
    }
}
