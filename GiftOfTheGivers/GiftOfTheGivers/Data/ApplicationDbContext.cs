using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Donation> Donations => Set<Donation>();
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    public DbSet<ReliefProject> ReliefProjects => Set<ReliefProject>();
    public DbSet<ProjectUpdate> ProjectUpdates => Set<ProjectUpdate>();
    public DbSet<TaxCertificate> TaxCertificates => Set<TaxCertificate>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Donation>()
            .Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Entity<Donation>()
            .HasOne(x => x.User)
            .WithMany(x => x.Donations)
            .HasForeignKey(x => x.UserID)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Volunteer>()
            .HasOne(x => x.User)
            .WithMany(x => x.VolunteerProfiles)
            .HasForeignKey(x => x.UserID)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<ProjectUpdate>()
            .HasOne(x => x.Project)
            .WithMany(x => x.Updates)
            .HasForeignKey(x => x.ProjectID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProjectUpdate>()
            .HasOne(x => x.Employee)
            .WithMany(x => x.ProjectUpdates)
            .HasForeignKey(x => x.EmployeeID)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<TaxCertificate>()
            .HasOne(x => x.Donation)
            .WithOne(x => x.TaxCertificate)
            .HasForeignKey<TaxCertificate>(x => x.DonationID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Donation>()
            .HasIndex(x => x.DonationDate);

        builder.Entity<Donation>()
            .HasIndex(x => x.Currency);

        builder.Entity<Volunteer>()
            .HasIndex(x => x.Status);

        builder.Entity<Volunteer>()
            .HasIndex(x => x.DateRegistered);

        builder.Entity<ReliefProject>()
            .HasIndex(x => x.Status);
    }
}
