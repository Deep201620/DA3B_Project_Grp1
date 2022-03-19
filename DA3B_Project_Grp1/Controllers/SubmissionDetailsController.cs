using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DA3B_Project_Grp1.Data;
using DA3B_Project_Grp1.Models;

namespace DA3B_Project_Grp1.Controllers
{
    public class SubmissionDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubmissionDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubmissionDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Submissions.Include(s => s.User).Include(s => s.project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SubmissionDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submissionDetails = await _context.Submissions
                .Include(s => s.User)
                .Include(s => s.project)
                .FirstOrDefaultAsync(m => m.SubmissionId == id);
            if (submissionDetails == null)
            {
                return NotFound();
            }

            return View(submissionDetails);
        }

        // GET: SubmissionDetails/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "DisplayName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectDescription");
            return View();
        }

        // POST: SubmissionDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,SubmissionId,ProjectId,SubmissionDate,SubmittedFileName,ApprovalStatus,ReviewedBy,Remarks")] SubmissionDetails submissionDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(submissionDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "DisplayName", submissionDetails.UserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectDescription", submissionDetails.ProjectId);
            return View(submissionDetails);
        }

        // GET: SubmissionDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submissionDetails = await _context.Submissions.FindAsync(id);
            if (submissionDetails == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "DisplayName", submissionDetails.UserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectDescription", submissionDetails.ProjectId);
            return View(submissionDetails);
        }

        // POST: SubmissionDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,SubmissionId,ProjectId,SubmissionDate,SubmittedFileName,ApprovalStatus,ReviewedBy,Remarks")] SubmissionDetails submissionDetails)
        {
            if (id != submissionDetails.SubmissionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(submissionDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubmissionDetailsExists(submissionDetails.SubmissionId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "DisplayName", submissionDetails.UserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectDescription", submissionDetails.ProjectId);
            return View(submissionDetails);
        }

        // GET: SubmissionDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submissionDetails = await _context.Submissions
                .Include(s => s.User)
                .Include(s => s.project)
                .FirstOrDefaultAsync(m => m.SubmissionId == id);
            if (submissionDetails == null)
            {
                return NotFound();
            }

            return View(submissionDetails);
        }

        // POST: SubmissionDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var submissionDetails = await _context.Submissions.FindAsync(id);
            _context.Submissions.Remove(submissionDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubmissionDetailsExists(int id)
        {
            return _context.Submissions.Any(e => e.SubmissionId == id);
        }
    }
}
