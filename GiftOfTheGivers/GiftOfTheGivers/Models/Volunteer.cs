using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class Volunteer
    {
        public int VolunteerID { get; set; }

        public string? UserID { get; set; }

        public ApplicationUser? User { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Skills { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Availability { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Province { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
    }
}