using System.ComponentModel.DataAnnotations;

namespace WebUI.Models;

public class EditTagViewModel
{
    [Required]
    public string TagName { get; set; } = string.Empty;
    public string? Note { get; set; }
}
