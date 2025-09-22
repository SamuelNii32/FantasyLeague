using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;



namespace Infrastructure.Data.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<Domain.Entities.User>


{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users");
        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasDefaultValueSql("NEWID()");
        b.Property(x => x.Username).IsRequired().HasMaxLength(50);
        b.Property(x => x.FirstName).IsRequired().HasMaxLength(60);
        b.Property(x => x.LastName).IsRequired().HasMaxLength(60);
        b.Property(x => x.Email).IsRequired().HasMaxLength(255);
        b.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255);
        b.Property(x => x.Role)
            .HasConversion<short>()
            .HasDefaultValue(UserRole.User)   
            .IsRequired();

        b.Property(x => x.isActive).HasDefaultValue(true).IsRequired();


        b.Property(x => x.CreatedAt)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        b.HasIndex(x => x.Username).IsUnique();
        b.HasIndex(x => x.Email).IsUnique();

        b.ToTable(t => t.HasCheckConstraint("CK_Users_RoleRange", "[Role] IN (0,1)"));
    }

}
