using Microsoft.AspNetCore.Mvc;
using Service;
using BussinessObject.Models;
using Microsoft.AspNetCore.OData.Query;
using API.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsArticleController : ControllerBase
{
    private readonly NewsArticleService _service = new();

    /// <remarks>
    /// <b>OData Query Options Supported:</b><br/>
    /// <ul>
    /// <li><code>$orderby</code> (e.g. <code>?$orderby=CreatedDate desc</code>)</li>
    /// <li><code>$top</code> (e.g. <code>?$top=10</code>)</li>
    /// <li><code>$skip</code> (e.g. <code>?$skip=10</code>)</li>
    /// <li><code>$filter</code> (e.g. <code>?$filter=NewsStatus eq true</code>)</li>
    /// <li><code>search</code> (e.g. <code>?search=keyword</code> for title/headline)</li>
    /// </ul>
    /// </remarks>
    /// <summary>
    /// Gets all news articles. Supports OData query options: $orderby, $top, $skip, $filter, and a custom 'search' parameter for searching by title or headline.
    /// Example: /api/NewsArticle?$orderby=CreatedDate desc&$top=10&search=keyword
    /// </summary>
    /// <param name="search">Optional search term for NewsTitle or Headline</param>
    /// <returns>Queryable list of NewsArticle</returns>
    [HttpGet]
    [EnableQuery]
    public IQueryable<NewsArticleDto> GetAll([FromQuery] string? search)
    {
        var query = _service.GetAll().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(n => n.NewsTitle.Contains(search) || n.Headline.Contains(search));
        }
        return query.Select(NewsArticleMapper.ToDto).AsQueryable();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var item = _service.GetById(id);
        if (item == null) return NotFound();
        return Ok(NewsArticleMapper.ToDto(item));
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
