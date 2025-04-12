namespace PosKu.Models
{
    public class RoleMenu
    {
        public string? RoleId { get; set; }
        public Role? Role { get; set; }

        public int MenuId { get; set; }
        public Menu? Menu { get; set; }
    }
}
