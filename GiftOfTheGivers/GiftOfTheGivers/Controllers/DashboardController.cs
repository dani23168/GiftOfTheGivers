using GiftOfTheGivers.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers;

[Authorize(Roles = "Employee")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _db;

    public DashboardController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.VolunteerCount = await _db.Volunteers.CountAsync();
        ViewBag.DonationCount = await _db.Donations.CountAsync();
        ViewBag.ProjectCount = await _db.ReliefProjects.CountAsync();
        ViewBag.TotalDonations = await _db.Donations.SumAsync(x => (decimal?)x.Amount) ?? 0;

        ViewBag.Volunteers = await _db.Volunteers
            .OrderByDescending(x => x.DateRegistered)
            .Take(10)
            .ToListAsync();

        ViewBag.Projects = await _db.ReliefProjects
            .Include(x => x.Updates)
            .OrderByDescending(x => x.StartDate)
            .ToListAsync();

        return View();
    }
}
