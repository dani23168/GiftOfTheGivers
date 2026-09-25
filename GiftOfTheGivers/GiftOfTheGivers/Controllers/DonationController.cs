using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers;

public class DonationController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _users;

    public DonationController(ApplicationDbContext db, UserManager<ApplicationUser> users)
    {
        _db = db;
        _users = users;
    }

    [HttpGet]
    public IActionResult Create() => View(new Donation());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Donation donation)
    {
        if (!new[] { "ZAR", "USD", "EUR" }.Contains(donation.Currency))
            ModelState.AddModelError(nameof(donation.Currency), "Please choose a supported currency.");

        if (!new[] { "One-Time", "Recurring" }.Contains(donation.DonationType))
            ModelState.AddModelError(nameof(donation.DonationType), "Please choose a supported donation type.");

        if (!ModelState.IsValid)
            return View(donation);

        if (User.Identity?.IsAuthenticated == true)
            donation.UserID = _users.GetUserId(User);

        donation.DonationDate = DateTime.UtcNow;
        _db.Donations.Add(donation);
        await _db.SaveChangesAsync();

        var certificate = new TaxCertificate
        {
            DonationID = donation.DonationID,
            CertificateNumber = $"GOTG-{DateTime.UtcNow:yyyyMMdd}-{donation.DonationID:000000}",
            GeneratedDate = DateTime.UtcNow,
            PDFLocation = "Prototype on-screen certificate"
        };

        _db.TaxCertificates.Add(certificate);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Success), new { id = donation.DonationID });
    }

    [HttpGet]
    public async Task<IActionResult> Success(int id)
    {
        var donation = await _db.Donations
            .Include(x => x.TaxCertificate)
            .FirstOrDefaultAsync(x => x.DonationID == id);

        return donation == null ? NotFound() : View(donation);
    }

    [HttpGet]
    public async Task<IActionResult> Certificate(int id)
    {
        var donation = await _db.Donations
            .Include(x => x.TaxCertificate)
            .FirstOrDefaultAsync(x => x.DonationID == id);

        return donation == null ? NotFound() : View(donation);
    }
}
