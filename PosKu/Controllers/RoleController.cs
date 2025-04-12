using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosKu.Context;
using PosKu.Models;

namespace PosKu.Controllers
{
    public class RoleController : Controller
    {
        private readonly AppDbContext _context;

        public RoleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.Include(r => r.RoleMenus)
                                            .ThenInclude(rm => rm.Menu)
                                            .ToListAsync();
            return View(roles);
        }

        // GET: Role/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var role = await _context.Roles
                .Include(r => r.RoleMenus)
                .ThenInclude(rm => rm.Menu)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Role/Create
        public IActionResult Create()
        {
            ViewBag.Menus = _context.Menus.ToList();
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role role, int[] selectedMenus)
        {
            if (ModelState.IsValid)
            {
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                // Assign selected menus to the role
                foreach (var menuId in selectedMenus)
                {
                    _context.RoleMenus.Add(new RoleMenu
                    {
                        RoleId = role.Id,
                        MenuId = menuId
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Menus = _context.Menus.ToList();
            return View(role);
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _context.Roles.Include(r => r.RoleMenus)
                                            .ThenInclude(rm => rm.Menu)
                                            .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
            {
                return NotFound();
            }

            ViewBag.Menus = _context.Menus.ToList();
            return View(role);
        }

        // POST: Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Role role, int[] selectedMenus)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(role);
                await _context.SaveChangesAsync();

                // Remove existing role-menu assignments and add new ones
                var existingRoleMenus = _context.RoleMenus.Where(rm => rm.RoleId == role.Id).ToList();
                _context.RoleMenus.RemoveRange(existingRoleMenus);

                foreach (var menuId in selectedMenus)
                {
                    _context.RoleMenus.Add(new RoleMenu
                    {
                        RoleId = role.Id,
                        MenuId = menuId
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Menus = _context.Menus.ToList();
            return View(role);
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role != null)
            {
                // Remove related RoleMenus
                var roleMenus = _context.RoleMenus.Where(rm => rm.RoleId == id).ToList();
                _context.RoleMenus.RemoveRange(roleMenus);

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}