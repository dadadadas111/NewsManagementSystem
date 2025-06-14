using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using WebUI.Models;
using System.Text;
using System.Net.Http.Headers;

namespace WebUI.Controllers;

public class CategoryDto
{
    public short CategoryId { get; set; }
    public string? CategoryName { get; set; }
}

public class TagDto
{
    public int TagId { get; set; }
    public string? TagName { get; set; }
}

public class NewsArticleViewModel
{
    public string? NewsArticleId { get; set; }
    public string? NewsTitle { get; set; }
    public string? Headline { get; set; }
    public string? NewsContent { get; set; }
    public string? NewsSource { get; set; }
    public string? CategoryName { get; set; }
    public bool? NewsStatus { get; set; }
    public string? CreatedDate { get; set; }
    public CategoryDto? Category { get; set; }
    public List<TagDto>? Tags { get; set; }
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
        if (articles != null)
        {
            foreach (var article in articles)
            {
                if (article?.Category != null)
                    article.CategoryName = article.Category.CategoryName;
            }
        }
        return View(articles);
    }

    public async Task<IActionResult> Details(string id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"https://localhost:7100/api/NewsArticle/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }
        var json = await response.Content.ReadAsStringAsync();
        var article = JsonSerializer.Deserialize<NewsArticleViewModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (article?.Category != null)
            article.CategoryName = article.Category.CategoryName;
        return View(article);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var client = _httpClientFactory.CreateClient();
        // Fetch categories
        var response = await client.GetAsync("https://localhost:7100/api/Category");
        var json = await response.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<List<CategoryDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "CategoryId", "CategoryName");
        // Fetch tags
        var tagResponse = await client.GetAsync("https://localhost:7100/api/Tag");
        var tagJson = await tagResponse.Content.ReadAsStringAsync();
        var tags = JsonSerializer.Deserialize<List<TagDto>>(tagJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ViewBag.Tags = new Microsoft.AspNetCore.Mvc.Rendering.MultiSelectList(tags, "TagId", "TagName");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateNewsArticleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7100/api/Category");
            var json = await response.Content.ReadAsStringAsync();
            var categories = JsonSerializer.Deserialize<List<CategoryDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "CategoryId", "CategoryName");
            var tagResponse = await client.GetAsync("https://localhost:7100/api/Tag");
            var tagJson = await tagResponse.Content.ReadAsStringAsync();
            var tags = JsonSerializer.Deserialize<List<TagDto>>(tagJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            ViewBag.Tags = new Microsoft.AspNetCore.Mvc.Rendering.MultiSelectList(tags, "TagId", "TagName");
            return View(model);
        }
        var clientPost = _httpClientFactory.CreateClient();
        var jsonPost = JsonSerializer.Serialize(model);
        var content = new StringContent(jsonPost, Encoding.UTF8, "application/json");
        var responsePost = await clientPost.PostAsync("https://localhost:7100/api/NewsArticle", content);
        if (responsePost.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        var error = await responsePost.Content.ReadAsStringAsync();
        ViewBag.ApiError = error;
        // Repopulate select lists on error
        var client2 = _httpClientFactory.CreateClient();
        var response2 = await client2.GetAsync("https://localhost:7100/api/Category");
        var json2 = await response2.Content.ReadAsStringAsync();
        var categories2 = JsonSerializer.Deserialize<List<CategoryDto>>(json2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories2, "CategoryId", "CategoryName");
        var tagResponse2 = await client2.GetAsync("https://localhost:7100/api/Tag");
        var tagJson2 = await tagResponse2.Content.ReadAsStringAsync();
        var tags2 = JsonSerializer.Deserialize<List<TagDto>>(tagJson2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ViewBag.Tags = new Microsoft.AspNetCore.Mvc.Rendering.MultiSelectList(tags2, "TagId", "TagName");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"https://localhost:7100/api/NewsArticle/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }
        var json = await response.Content.ReadAsStringAsync();
        var article = JsonSerializer.Deserialize<NewsArticleViewModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (article == null)
        {
            return NotFound();
        }
        var model = new EditNewsArticleViewModel
        {
            NewsTitle = article.NewsTitle,
            Headline = article.Headline,
            NewsContent = article.NewsContent,
            NewsSource = article.NewsSource,
            CategoryId = article.Category?.CategoryId,
            NewsStatus = article.NewsStatus,
            TagIds = article.Tags?.ConvertAll(t => t.TagId) ?? new List<int>()
        };
        // Fetch categories and tags for select lists
        var catResponse = await client.GetAsync("https://localhost:7100/api/Category");
        var catJson = await catResponse.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<List<CategoryDto>>(catJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "CategoryId", "CategoryName");
        var tagResponse = await client.GetAsync("https://localhost:7100/api/Tag");
        var tagJson = await tagResponse.Content.ReadAsStringAsync();
        var tags = JsonSerializer.Deserialize<List<TagDto>>(tagJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ViewBag.Tags = new Microsoft.AspNetCore.Mvc.Rendering.MultiSelectList(tags, "TagId", "TagName");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, EditNewsArticleViewModel model)
    {
        var updateDto = new
        {
            NewsTitle = model.NewsTitle,
            Headline = model.Headline,
            NewsContent = model.NewsContent,
            NewsSource = model.NewsSource,
            CategoryId = model.CategoryId,
            NewsStatus = model.NewsStatus,
            TagIds = model.TagIds
        };
        var client = _httpClientFactory.CreateClient();
        var json = JsonSerializer.Serialize(updateDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"https://localhost:7100/api/NewsArticle/{id}", content);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        var error = await response.Content.ReadAsStringAsync();
        ViewBag.ApiError = error;
        // Repopulate select lists on error
        var catResponse = await client.GetAsync("https://localhost:7100/api/Category");
        var catJson = await catResponse.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<List<CategoryDto>>(catJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "CategoryId", "CategoryName");
        var tagResponse = await client.GetAsync("https://localhost:7100/api/Tag");
        var tagJson = await tagResponse.Content.ReadAsStringAsync();
        var tags = JsonSerializer.Deserialize<List<TagDto>>(tagJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ViewBag.Tags = new Microsoft.AspNetCore.Mvc.Rendering.MultiSelectList(tags, "TagId", "TagName");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.DeleteAsync($"https://localhost:7100/api/NewsArticle/{id}");
        // Optionally handle errors
        return RedirectToAction("Index");
    }
}
