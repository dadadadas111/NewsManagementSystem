using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers;

public class NewsArticleHistoryController : Controller
{
    public IActionResult Index()
    {
        // TODO: Show news articles created by current staff user
        return View();
    }
}
