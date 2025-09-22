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
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> b)
        {
            b.ToTable("Teams");

            // Key
            b.HasKey(x => x.TeamId);
            b.Property(x => x.TeamId).HasDefaultValueSql("NEWID()");

            // Required + lengths
            b.Property(x => x.TeamName).IsRequired().HasMaxLength(100);
            b.Property(x => x.UserId).IsRequired();

            // Defaults
            b.Property(x => x.BudgetRemaining).HasDefaultValue(1000);          
            b.Property(x => x.TotalPoints).HasDefaultValue(0);
            b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

            // FK to Users — Restrict (we’ll soft-delete users to preserve history)
            b.HasOne(x => x.User)
             .WithMany()                          
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            // Helpful index for queries
            b.HasIndex(x => x.UserId);

            // Basic integrity checks
            b.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Teams_BudgetRemaining_NonNeg", "[BudgetRemaining] >= 0");
                t.HasCheckConstraint("CK_Teams_TotalPoints_NonNeg", "[TotalPoints] >= 0");
            });
        }
    }
}
