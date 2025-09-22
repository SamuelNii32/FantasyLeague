using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class LeagueConfiguration : IEntityTypeConfiguration<League>
{
    public void Configure(EntityTypeBuilder<League> b)
    {
        b.ToTable("Leagues");

        // Key
        b.HasKey(x => x.LeagueId);
        b.Property(x => x.LeagueId).HasDefaultValueSql("NEWID()");

        // Columns
        b.Property(x => x.LeagueName).IsRequired().HasMaxLength(150);
        b.Property(x => x.CreatedBy).IsRequired();

        // NEW: required fields
        b.Property(x => x.StartGameweekId).IsRequired();

        b.Property(x => x.JoinCode)
         .IsRequired()
         .HasMaxLength(12);

        // Enum stored as tinyint/byte; default Classic (use enum value, not numeric)
        b.Property(x => x.Type)
         .HasConversion<byte>()
         .HasDefaultValue(LeagueType.Classic)
         .IsRequired();

        // Optional
        b.Property(x => x.MaxTeams); // nullable

        // Timestamps
        b.Property(x => x.CreatedAt)
         .HasDefaultValueSql("SYSUTCDATETIME()")
         .IsRequired();

        // FK: CreatedBy -> Users(Id), do NOT cascade (preserve league history)
        b.HasOne(x => x.CreatedByUser)
         .WithMany()
         .HasForeignKey(x => x.CreatedBy)
         .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        b.HasIndex(x => x.JoinCode).IsUnique();
        b.HasIndex(x => x.CreatedBy);
        b.HasIndex(x => new { x.Type, x.StartGameweekId });

        // Table-level checks
        b.ToTable(t =>
        {
            // Ensure JoinCode length is between 8 and 12
            t.HasCheckConstraint("CK_Leagues_JoinCode_Length", "LEN([JoinCode]) BETWEEN 8 AND 12");

            // Optional: Classic = NULL; H2H requires 4–20 and even
            t.HasCheckConstraint(
                "CK_Leagues_Type_MaxTeams",
                "([Type] = 0 AND [MaxTeams] IS NULL) OR " +
                "([Type] = 1 AND [MaxTeams] IS NOT NULL AND [MaxTeams] BETWEEN 4 AND 20 AND ([MaxTeams] % 2) = 0)"
            );
        });
    }
}

