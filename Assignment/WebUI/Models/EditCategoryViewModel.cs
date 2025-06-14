using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class EditCategoryViewModel
    {
        [Required]
        [StringLength(100)]
        public string? CategoryName { get; set; }

        [StringLength(400)]
        public string? CategoryDescription { get; set; }
    }
}
