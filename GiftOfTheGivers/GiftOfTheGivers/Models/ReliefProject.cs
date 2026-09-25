using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GiftOfTheGivers.Models
{
    public class ReliefProject
    {
        [Key]
        public int ProjectID { get; set; }

        public string ProjectName { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public string Province { get; set; } = string.Empty;

        public string DisasterType { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Status { get; set; } = "Active";

        public ICollection<ProjectUpdate> Updates { get; set; } = new List<ProjectUpdate>();
    }
}