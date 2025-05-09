﻿namespace PosKu.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Path { get; set; }
        public ICollection<RoleMenu>? RoleMenus { get; set; } = [];
    }
}
