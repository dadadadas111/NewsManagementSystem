using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using WebUI.Models;

namespace WebUI.Controllers;

public class AuthController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public AuthController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var client = _httpClientFactory.CreateClient();
        var json = JsonSerializer.Serialize(new { Username = model.Username, Password = model.Password });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:7100/api/auth/login", content);
        if (response.IsSuccessStatusCode)
        {
            var respJson = await response.Content.ReadAsStringAsync();
            var loginResp = JsonSerializer.Deserialize<LoginResponseViewModel>(respJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            // Store token and role in session
            HttpContext.Session.SetString("JWToken", loginResp.Token);
            HttpContext.Session.SetString("UserRole", loginResp.Role);
            HttpContext.Session.SetString("Username", loginResp.Username);
            HttpContext.Session.SetString("AccountId", loginResp.AccountId?.ToString() ?? "");
            return RedirectToAction("Index", "Home");
        }
        var error = await response.Content.ReadAsStringAsync();
        ViewBag.ApiError = error;
        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
