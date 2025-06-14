using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace WebUI.Controllers;

public class NewsArticleHistoryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public NewsArticleHistoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();
        var token = HttpContext.Session.GetString("JWToken");
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await client.GetAsync("https://localhost:7100/api/NewsArticle/mine");
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.ApiError = await response.Content.ReadAsStringAsync();
            return View(new List<NewsArticleViewModel>());
        }
        var json = await response.Content.ReadAsStringAsync();
        var articles = JsonSerializer.Deserialize<List<NewsArticleViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return View(articles ?? new List<NewsArticleViewModel>());
    }
}
