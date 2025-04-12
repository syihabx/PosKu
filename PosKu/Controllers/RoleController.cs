using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosKu.Context;

namespace PosKu.Controllers
{
    public class RoleController : Controller
    {
        private readonly AppDbContext _context;
        public RoleController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.Include(r=>r.RoleMenus)
                .ThenInclude(m=>m.Menu).ToListAsync();
            return View(roles);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await _context.Roles
                .Include(r => r.RoleMenus)
                .ThenInclude(m => m.Menu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }
    }
}
