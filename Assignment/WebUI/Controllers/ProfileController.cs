using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Controllers;

public class ProfileController : Controller
{
    public IActionResult Index()
    {
        // TODO: Load current user's profile from API using AccountId from session
        return View();
    }
    [HttpPost]
    public IActionResult Update(ProfileViewModel model)
    {
        // TODO: Update profile via API
        return RedirectToAction("Index");
    }
}
