using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [StringLength(400)]
        public string? CategoryDescription { get; set; }

        public short? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
    }
}
