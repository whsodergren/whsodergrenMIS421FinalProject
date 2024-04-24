using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace whsodergrenMIS421FinalProject.Models
{
    public class UsersAndRoles
    {
        public IEnumerable<IdentityUser> Users { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
        public Dictionary<string, string> UserRoles { get; set; }
        public SelectList RoleList { get; set; }
    }
}
