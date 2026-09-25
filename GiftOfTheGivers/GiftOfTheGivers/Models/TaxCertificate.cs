using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class TaxCertificate
    {
        public int CertificateID { get; set; }

        public int DonationID { get; set; }

        public Donation? Donation { get; set; }

        [Required]
        [StringLength(50)]
        public string CertificateNumber { get; set; } = string.Empty;

        public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;

        public string? PDFLocation { get; set; }
    }
}
