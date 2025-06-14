using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UpdateSystemAccountDto
    {
        [Required]
        [StringLength(100)]
        public string? AccountName { get; set; }

        [Required]
        [EmailAddress]
        public string? AccountEmail { get; set; }

        [Required]
        [Range(1, 2)] // 1=Admin, 2=User
        public int AccountRole { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string? AccountPassword { get; set; }
    }
}
