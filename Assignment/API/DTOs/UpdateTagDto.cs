using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UpdateTagDto
    {
        [Required]
        [StringLength(100)]
        public string? TagName { get; set; }

        [StringLength(400)]
        public string? Note { get; set; }
    }
}
