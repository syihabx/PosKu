using Microsoft.AspNetCore.Identity;

namespace PosKu.Models
{
    public class Role: IdentityRole
    {
        public string Description { get; set; }
        public virtual ICollection<User>? Users { get; set; } = [];
        public virtual ICollection<RoleMenu> RoleMenus { get; set; } = [];
    }
}
