using Microsoft.AspNetCore.Mvc;
using Service;
using BussinessObject.Models;
using Microsoft.AspNetCore.OData.Query;
using API.DTOs;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsArticleController : ControllerBase
{
    private readonly NewsArticleService _service = new();

    [HttpGet]
    [EnableQuery]
    public IQueryable<NewsArticleDto> GetAll([FromQuery] string? search)
    {
        var query = _service.GetAll().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(n => (n.NewsTitle != null && n.NewsTitle.Contains(search)) || (n.Headline != null && n.Headline.Contains(search)));
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
    public IActionResult Add([FromBody] CreateNewsArticleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        // Generate a unique ID based on current time (yyyyMMddHHmmssfff + random digits if needed)
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        var random = new Random();
        var uniqueId = timestamp + random.Next(100, 999).ToString(); // 20 chars max
        var newsArticle = new NewsArticle
        {
            NewsArticleId = uniqueId,
            NewsTitle = dto.NewsTitle ?? string.Empty,
            Headline = dto.Headline ?? string.Empty,
            NewsContent = dto.NewsContent,
            NewsSource = dto.NewsSource,
            CategoryId = dto.CategoryId,
            NewsStatus = dto.NewsStatus,
            CreatedById = dto.CreatedById,
            CreatedDate = DateTime.UtcNow,
            // Tags will be set below
        };
        // Attach tags if provided (use service/repository/DAO layer)
        // Use EntityState.Unchanged to avoid EF trying to insert duplicate Tag PKs
        if (dto.TagIds != null && dto.TagIds.Count > 0)
        {
            foreach (var tagId in dto.TagIds.Distinct())
            {
                var tag = new BussinessObject.Models.Tag { TagId = tagId };
                newsArticle.Tags.Add(tag);
            }
        }
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
