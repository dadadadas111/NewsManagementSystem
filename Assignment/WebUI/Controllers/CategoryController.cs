using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using WebUI.Models;
using System.Text;
using System.Net.Http.Headers;

namespace WebUI.Controllers;

public class CategoryViewModel
{
    public short CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryDescription { get; set; } = string.Empty;
}

public class CategoryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public CategoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private void AttachJwt(HttpClient client)
    {
        var token = HttpContext.Session.GetString("JWToken");
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<IActionResult> Index(string? orderby, int? top, int? skip)
    {
        var client = _httpClientFactory.CreateClient();
        AttachJwt(client);
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

    public async Task<IActionResult> Details(short id)
    {
        var client = _httpClientFactory.CreateClient();
        AttachJwt(client);
        var response = await client.GetAsync($"https://localhost:7100/api/Category/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }
        var json = await response.Content.ReadAsStringAsync();
        var category = JsonSerializer.Deserialize<CategoryViewModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("https://localhost:7100/api/Category");
        var json = await response.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<List<CategoryViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ViewBag.ParentCategories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "CategoryId", "CategoryName");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var client2 = _httpClientFactory.CreateClient();
            var response2 = await client2.GetAsync("https://localhost:7100/api/Category");
            var json2 = await response2.Content.ReadAsStringAsync();
            var categories2 = JsonSerializer.Deserialize<List<CategoryViewModel>>(json2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            ViewBag.ParentCategories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories2, "CategoryId", "CategoryName");
            return View(model);
        }
        var clientPost = _httpClientFactory.CreateClient();
        AttachJwt(clientPost);
        var jsonPost = JsonSerializer.Serialize(model);
        var content = new StringContent(jsonPost, Encoding.UTF8, "application/json");
        var responsePost = await clientPost.PostAsync("https://localhost:7100/api/Category", content);
        if (responsePost.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        var error = await responsePost.Content.ReadAsStringAsync();
        ViewBag.ApiError = error;
        var client3 = _httpClientFactory.CreateClient();
        var response3 = await client3.GetAsync("https://localhost:7100/api/Category");
        var json3 = await response3.Content.ReadAsStringAsync();
        var categories3 = JsonSerializer.Deserialize<List<CategoryViewModel>>(json3, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ViewBag.ParentCategories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories3, "CategoryId", "CategoryName");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(short id)
    {
        var client = _httpClientFactory.CreateClient();
        AttachJwt(client);
        var response = await client.GetAsync($"https://localhost:7100/api/Category/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }
        var json = await response.Content.ReadAsStringAsync();
        var category = JsonSerializer.Deserialize<CategoryViewModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (category == null)
        {
            return NotFound();
        }
        var model = new EditCategoryViewModel
        {
            CategoryName = category.CategoryName,
            CategoryDescription = category.CategoryDescription
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(short id, EditCategoryViewModel model)
    {
        var client = _httpClientFactory.CreateClient();
        AttachJwt(client);
        var updateDto = new
        {
            CategoryName = model.CategoryName,
            CategoryDescription = model.CategoryDescription
        };
        var json = JsonSerializer.Serialize(updateDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"https://localhost:7100/api/Category/{id}", content);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        var error = await response.Content.ReadAsStringAsync();
        ViewBag.ApiError = error;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(short id)
    {
        var client = _httpClientFactory.CreateClient();
        AttachJwt(client);
        var response = await client.DeleteAsync($"https://localhost:7100/api/Category/{id}");
        // Optionally handle errors
        return RedirectToAction("Index");
    }
}
