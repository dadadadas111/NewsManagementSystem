using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateSystemAccountDto
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

        [Range(1, short.MaxValue, ErrorMessage = "AccountId must be a positive number")]
        public short? AccountId { get; set; } // Optional: allow user to supply AccountId
    }
}
