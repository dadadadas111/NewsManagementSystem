using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class CreateTagViewModel
    {
        [Required]
        [StringLength(100)]
        public string? TagName { get; set; }

        [StringLength(400)]
        public string? Note { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "TagId must be a positive integer")]
        public int? TagId { get; set; } // Optional: allow user to supply TagId
    }
}
