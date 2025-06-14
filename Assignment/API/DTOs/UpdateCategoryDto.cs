using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UpdateCategoryDto
    {
        [Required]
        [StringLength(100)]
        public string? CategoryName { get; set; }

        [StringLength(400)]
        public string? CategoryDescription { get; set; }
    }
}
