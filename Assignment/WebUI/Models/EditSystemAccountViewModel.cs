using System.ComponentModel.DataAnnotations;

namespace WebUI.Models;

public class EditSystemAccountViewModel
{
    [Required]
    public string AccountName { get; set; } = string.Empty;
    [Required]
    public string AccountEmail { get; set; } = string.Empty;
    [Required]
    public int AccountRole { get; set; }
    [Required]
    public string AccountPassword { get; set; } = string.Empty;
}
