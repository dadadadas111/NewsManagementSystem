using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers;

public class ReportController : Controller
{
    public IActionResult Index()
    {
        // TODO: Show report/statistic UI for admin
        return View();
    }
    [HttpPost]
    public IActionResult Generate(string startDate, string endDate)
    {
        // TODO: Call API to get report data
        return View("Index");
    }
}
