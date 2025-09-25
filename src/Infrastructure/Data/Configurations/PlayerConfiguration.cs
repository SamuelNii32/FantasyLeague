using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            // Table
            builder.ToTable("Players");

            // Key (DB generates GUIDs so SSMS inserts work without typing Id)
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                   .HasDefaultValueSql("NEWSEQUENTIALID()");

            // Required + lengths
            builder.Property(p => p.FirstName)
                   .IsRequired()
                   .HasMaxLength(60);

            builder.Property(p => p.LastName)
                   .IsRequired()
                   .HasMaxLength(60);

            builder.Property(p => p.Position)
                   .IsRequired()
                   .HasConversion<short>()        
                   .HasColumnType("smallint");

            builder.Property(p => p.Status)
                   .IsRequired()
                   .HasConversion<short>()        
                   .HasColumnType("smallint");

            builder.Property(p => p.Cost)
                   .IsRequired();                  

            builder.Property(p => p.ClubId)
                   .IsRequired();

            // Default timestamp (UTC)
            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            // FK: Players.ClubId → Clubs.Id (NoAction = block delete if dependents exist)
            builder.HasOne(p => p.Club)
                   .WithMany(c => c.Players)
                   .HasForeignKey(p => p.ClubId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(p => new { p.ClubId, p.Position }); // squad/transfer filters

            // Optional: quick search by last name
            builder.HasIndex(p => p.LastName);


            // Check constraints (DB-level guards)
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Players_Position", "[Position] IN (0,1,2,3)");
                t.HasCheckConstraint("CK_Players_Status", "[Status]   IN (0,1,2,3)");
                t.HasCheckConstraint("CK_Players_Cost", "[Cost] BETWEEN 35 AND 150");
            });
        }
    }
}
