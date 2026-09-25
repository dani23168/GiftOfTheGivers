using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class Donation
    {
        public int DonationID { get; set; }

        public string? UserID { get; set; }

        public ApplicationUser? User { get; set; }

        [Required]
        [Range(1, 100000000)]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "ZAR";

        [Required]
        public string DonationType { get; set; } = "One-Time";

        public DateTime DonationDate { get; set; } = DateTime.UtcNow;

        public bool Anonymous { get; set; }

        public TaxCertificate? TaxCertificate { get; set; }
    }
}