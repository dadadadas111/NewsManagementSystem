using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace WebUI.Controllers;

public class NewsArticleViewModel
{
    public string NewsArticleId { get; set; }
    public string NewsTitle { get; set; }
    public string Headline { get; set; }
    public string? NewsContent { get; set; }
    public string? NewsSource { get; set; }
    public string? CategoryName { get; set; } // will be set manually after deserialization
    public bool? NewsStatus { get; set; }
    public string? CreatedDate { get; set; }
    public CategoryDto? Category { get; set; } // for extracting CategoryName
}

public class CategoryDto
{
    public short CategoryId { get; set; }
    public string? CategoryName { get; set; }
}

public class NewsArticleController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public NewsArticleController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index(string? search, string? orderby, int? top, int? page)
    {
        var client = _httpClientFactory.CreateClient();
        int pageSize = top ?? 10;
        int pageNumber = page ?? 1;
        if (pageNumber < 1) pageNumber = 1;
        int skip = (pageNumber - 1) * pageSize;
        var odataParams = new List<string>();
        if (!string.IsNullOrWhiteSpace(search)) odataParams.Add("search=" + Uri.EscapeDataString(search));
        if (!string.IsNullOrWhiteSpace(orderby)) odataParams.Add("$orderby=" + orderby);
        odataParams.Add("$top=" + pageSize);
        odataParams.Add("$skip=" + skip);
        string odataQuery = odataParams.Count > 0 ? ("?" + string.Join("&", odataParams)) : string.Empty;
        string url = $"https://localhost:7100/api/NewsArticle{odataQuery}";
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var articles = JsonSerializer.Deserialize<List<NewsArticleViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        foreach (var article in articles)
        {
            if (article.Category != null)
                article.CategoryName = article.Category.CategoryName;
        }
        return View(articles);
    }
}
