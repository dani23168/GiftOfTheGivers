using System;
using System.IO;
using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

try
{
    var builder = WebApplication.CreateBuilder(args);

    var provider = builder.Configuration["DatabaseProvider"] ?? "Sqlite";
    var connectionString = provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)
        ? (builder.Configuration.GetConnectionString("AzureSqlConnection")
           ?? builder.Configuration.GetConnectionString("DefaultConnection"))
        : builder.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("A database connection string is required.");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            options.UseSqlServer(connectionString);
        else
            options.UseSqlite(connectionString);
    });

    builder.Services
        .AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();

    var app = builder.Build();

    // Database migrations and seeding are disabled at startup to avoid startup-time
    // dependency-resolution issues on machines without a configured database. If
    // you need to apply migrations or seed the database, run the following from
    // the repository root in a terminal:
    //   dotnet ef database update --project "GiftOfTheGivers/GiftOfTheGivers.csproj"
    //   dotnet run --project "GiftOfTheGivers/GiftOfTheGivers.csproj" -- seed
    // The code previously attempted to run migrations at startup and caused DI
    // resolution errors on some environments; it's safer to perform these steps
    // explicitly during deployment or development.

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.MapRazorPages();

    app.Run();
}
catch (Exception ex)
{
    try
    {
        var logDir = Path.Combine(AppContext.BaseDirectory, "logs");
        Directory.CreateDirectory(logDir);
        var logPath = Path.Combine(logDir, "startup-error.log");
        File.AppendAllText(logPath, $"[{DateTime.UtcNow:o}] {ex}\n\n");
    }
    catch { /* best-effort logging */ }

    Console.Error.WriteLine("Fatal startup exception:\n" + ex);
    throw;
}
