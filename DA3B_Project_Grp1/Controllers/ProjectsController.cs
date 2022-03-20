using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DA3B_Project_Grp1.Data;
using DA3B_Project_Grp1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DA3B_Project_Grp1.Controllers
{
    [Authorize(Roles = "Student")]

    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<MyIdentityUser> _userManager;
       


        public ProjectsController(ApplicationDbContext context, UserManager<MyIdentityUser> userManager, SignInManager<MyIdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            var applicationDbContext = _context.Project.Include(p => p.User).Where(s => s.UserId.ToString() == userid);

            //var model = await _context.Projects
            //                .Where(a => a.User.Id.ToString() == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
            //                .ToListAsync();
            //return View(model);

            //var applicationDbContext = _context.Projects.Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());

        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            //Getting username by userId
            var userid = _userManager.GetUserId(HttpContext.User);
            //var name = _userManager.Users.FirstOrDefault(u => u.Id.ToString() == userid);
            ViewBag.UserId = userid;
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "DisplayName");
            //ViewData["UserId"] = userid;
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,ProjectId,ProjectTitle,ProjectDescription,StartDate,EndDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var userid = _userManager.GetUserId(HttpContext.User);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "DisplayName", project.UserId);
            //ViewData["UserId"] = userid;
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "DisplayName", project.UserId);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,ProjectId,ProjectTitle,ProjectDescription,StartDate,EndDate")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "DisplayName", project.UserId);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }

        [HttpGet]
        public async Task<string> GetCurrentUserId()
        {
            MyIdentityUser usr = await GetCurrentUserAsync();
            return usr?.Id.ToString();
        }
        private Task<MyIdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
