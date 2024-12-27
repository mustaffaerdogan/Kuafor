using Microsoft.AspNetCore.Identity;

namespace KuaforApp.Models
{
    // Custom user class that extends IdentityUser to add additional properties
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public UserRole Role { get; set; } = UserRole.User; // Default role is User
    }
} 