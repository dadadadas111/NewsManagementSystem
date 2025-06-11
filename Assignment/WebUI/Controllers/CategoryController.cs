using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace WebUI.Controllers;

public class CategoryViewModel
{
    public short CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategoryDesciption { get; set; }
}

public class CategoryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public CategoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index(string? orderby, int? top, int? skip)
    {
        var client = _httpClientFactory.CreateClient();
        var odataParams = new List<string>();
        if (!string.IsNullOrWhiteSpace(orderby)) odataParams.Add("$orderby=" + orderby);
        if (top.HasValue) odataParams.Add("$top=" + top);
        if (skip.HasValue) odataParams.Add("$skip=" + skip);
        string odataQuery = odataParams.Count > 0 ? ("?" + string.Join("&", odataParams)) : string.Empty;
        var response = await client.GetAsync($"https://localhost:7100/api/Category{odataQuery}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<List<CategoryViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return View(categories);
    }
}
