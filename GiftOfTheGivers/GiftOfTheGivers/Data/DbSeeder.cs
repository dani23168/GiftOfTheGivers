using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var db = services.GetRequiredService<ApplicationDbContext>();

        foreach (var role in new[] { "Employee", "Donor" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        const string email = "employee@giftofthegivers.org";
        const string password = "Employee123!";

        var employee = await userManager.FindByEmailAsync(email);
        if (employee == null)
        {
            employee = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FullName = "Gift of the Givers Employee"
            };

            var result = await userManager.CreateAsync(employee, password);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(employee, "Employee");
        }
        else if (!await userManager.IsInRoleAsync(employee, "Employee"))
        {
            await userManager.AddToRoleAsync(employee, "Employee");
        }

        if (!await db.ReliefProjects.AnyAsync())
        {
            db.ReliefProjects.AddRange(
                new ReliefProject
                {
                    ProjectName = "KwaZulu-Natal Flood Response",
                    Province = "KwaZulu-Natal",
                    DisasterType = "Flood",
                    StartDate = DateTime.UtcNow.AddDays(-14),
                    Status = "Active",
                    Summary = "Emergency food, water and shelter support for affected communities."
                },
                new ReliefProject
                {
                    ProjectName = "Community Food Relief",
                    Province = "Gauteng",
                    DisasterType = "Food Insecurity",
                    StartDate = DateTime.UtcNow.AddDays(-30),
                    Status = "Active",
                    Summary = "Distribution of food parcels to vulnerable households."
                }
            );
            await db.SaveChangesAsync();
        }
    }
}
