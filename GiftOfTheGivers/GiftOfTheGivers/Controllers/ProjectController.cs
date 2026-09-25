using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers;

public class ProjectController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _users;

    public ProjectController(ApplicationDbContext db, UserManager<ApplicationUser> users)
    {
        _db = db;
        _users = users;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var projects = await _db.ReliefProjects
            .Include(x => x.Updates)
            .OrderByDescending(x => x.StartDate)
            .ToListAsync();

        return View(projects);
    }

    [Authorize(Roles = "Employee")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Employee")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReliefProject project)
    {
        if (!ModelState.IsValid) return View(project);
        _db.ReliefProjects.Add(project);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Employee")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddUpdate(int projectId, string updateText)
    {
        if (string.IsNullOrWhiteSpace(updateText))
            return RedirectToAction(nameof(Index));

        var project = await _db.ReliefProjects.FindAsync(projectId);
        if (project == null) return NotFound();

        _db.ProjectUpdates.Add(new ProjectUpdate
        {
            ProjectID = projectId,
            EmployeeID = _users.GetUserId(User),
            UpdateText = updateText.Trim(),
            UpdateDate = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
