using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using WebUI.Models;
using System.Text;
using System.Net.Http.Headers;

namespace WebUI.Controllers;

public class SystemAccountViewModel
{
    public short AccountId { get; set; }
    public string? AccountName { get; set; }
    public string? AccountEmail { get; set; }
    public int? AccountRole { get; set; }
}

public class SystemAccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public SystemAccountController(IHttpClientFactory httpClientFactory)
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
        var response = await client.GetAsync($"https://localhost:7100/api/SystemAccount{odataQuery}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var accounts = JsonSerializer.Deserialize<List<SystemAccountViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return View(accounts);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSystemAccountViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var client = _httpClientFactory.CreateClient();
        var json = JsonSerializer.Serialize(new {
            AccountId = model.AccountId,
            AccountName = model.AccountName,
            AccountEmail = model.AccountEmail,
            AccountRole = model.AccountRole,
            AccountPassword = model.AccountPassword
        });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:7100/api/SystemAccount", content);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        var error = await response.Content.ReadAsStringAsync();
        ViewBag.ApiError = error;
        return View(model);
    }
}
