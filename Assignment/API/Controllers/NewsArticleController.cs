using Microsoft.AspNetCore.Mvc;
using Service;
using BussinessObject.Models;
using Microsoft.AspNetCore.OData.Query;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsArticleController : ControllerBase
{
    private readonly NewsArticleService _service = new();

    [HttpGet]
    [EnableQuery]
    public IQueryable<NewsArticle> GetAll([FromQuery] string? search)
    {
        var query = _service.GetAll().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(n => n.NewsTitle.Contains(search) || n.Headline.Contains(search));
        }
        return query;
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var item = _service.GetById(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public IActionResult Add(NewsArticle newsArticle)
    {
        _service.Add(newsArticle);
        return CreatedAtAction(nameof(GetById), new { id = newsArticle.NewsArticleId }, newsArticle);
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, NewsArticle newsArticle)
    {
        if (id != newsArticle.NewsArticleId) return BadRequest();
        _service.Update(newsArticle);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        _service.Delete(id);
        return NoContent();
    }
}
