using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum UserRole: short { User = 0, Admin = 1}
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool isActive { get; set; } = true;
    }
}
