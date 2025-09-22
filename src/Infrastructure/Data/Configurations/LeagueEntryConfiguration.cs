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

    public class LeagueEntryConfiguration : IEntityTypeConfiguration<LeagueEntry>
    {
        public void Configure(EntityTypeBuilder<LeagueEntry> b)
        {
            b.ToTable("LeagueEntries");

            // Primary key
            b.HasKey(x => x.LeagueEntryId);
            b.Property(x => x.LeagueEntryId).HasDefaultValueSql("NEWID()");

            // Required FKs
            b.Property(x => x.LeagueId).IsRequired();
            b.Property(x => x.TeamId).IsRequired();

            // Defaults
            b.Property(x => x.JoinedAt)
             .HasDefaultValueSql("SYSUTCDATETIME()");

            // FK: League -> Entries (CASCADE is safe: removes only membership rows)
            b.HasOne(x => x.League)
             .WithMany(l => l.Entries!)
             .HasForeignKey(x => x.LeagueId)
             .OnDelete(DeleteBehavior.Cascade);

            // FK: Team -> Entries (CASCADE is safe: removes only membership rows)
            b.HasOne(x => x.Team)
             .WithMany(t => t.LeagueEntries!)
             .HasForeignKey(x => x.TeamId)
             .OnDelete(DeleteBehavior.Cascade);

            // A team can join a league only once
            b.HasIndex(x => new { x.LeagueId, x.TeamId }).IsUnique();

            // Helpful indexes
            b.HasIndex(x => x.LeagueId);
            b.HasIndex(x => x.TeamId);
        }
    }

}
