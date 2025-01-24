using Microsoft.AspNetCore.Identity;

namespace UserManagementApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime LastLoginTime { get; set; }
        public bool IsBlocked { get; set; }
        public string? Name { get; set; }
    }

}
