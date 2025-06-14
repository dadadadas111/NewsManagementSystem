using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using WebUI.Models;
using System.Text;
using System.Net.Http.Headers;

namespace WebUI.Controllers;

public class TagViewModel
{
    public int TagId { get; set; }
    public string? TagName { get; set; }
    public string? Note { get; set; }
}

public class TagController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public TagController(IHttpClientFactory httpClientFactory)
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
        var response = await client.GetAsync($"https://localhost:7100/api/Tag{odataQuery}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var tags = JsonSerializer.Deserialize<List<TagViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return View(tags);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTagViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var client = _httpClientFactory.CreateClient();
        var json = JsonSerializer.Serialize(new {
            TagId = model.TagId,
            TagName = model.TagName,
            Note = model.Note
        });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:7100/api/Tag", content);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        var error = await response.Content.ReadAsStringAsync();
        ViewBag.ApiError = error;
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"https://localhost:7100/api/Tag/{id}");
        if (!response.IsSuccessStatusCode)
            return NotFound();
        var json = await response.Content.ReadAsStringAsync();
        var tag = JsonSerializer.Deserialize<CreateTagViewModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return View(tag);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CreateTagViewModel model)
    {
        var client = _httpClientFactory.CreateClient();
        var json = JsonSerializer.Serialize(new {
            TagId = model.TagId,
            TagName = model.TagName,
            Note = model.Note
        });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"https://localhost:7100/api/Tag/{model.TagId}", content);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        var error = await response.Content.ReadAsStringAsync();
        ViewBag.ApiError = error;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.DeleteAsync($"https://localhost:7100/api/Tag/{id}");
        // Optionally handle errors
        return RedirectToAction("Index");
    }
}
