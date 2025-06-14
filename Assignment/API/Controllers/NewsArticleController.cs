using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    [EnableQuery]
    public IQueryable<NewsArticleDto> GetAll([FromQuery] string? search)
    {
        var query = _service.GetAll().Where(n => n.NewsStatus == true).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(n => (n.NewsTitle != null && n.NewsTitle.Contains(search)) || (n.Headline != null && n.Headline.Contains(search)));
        }
        return query.Select(NewsArticleMapper.ToDto).AsQueryable();
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IActionResult GetById(string id)
    {
        var item = _service.GetById(id);
        if (item == null || item.NewsStatus != true) return NotFound();
        return Ok(NewsArticleMapper.ToDto(item));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrStaff")]
    public IActionResult Add([FromBody] CreateNewsArticleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        // Get author from JWT
        var userIdClaim = User.Claims.FirstOrDefault(c => string.Equals(c.Type, "AccountId", StringComparison.OrdinalIgnoreCase) || c.Type.EndsWith("/accountId", StringComparison.OrdinalIgnoreCase));
        if (userIdClaim == null || !short.TryParse(userIdClaim.Value, out var authorId))
            return Unauthorized("No valid accountId claim found in token.");
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
            CreatedById = authorId,
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
        return CreatedAtAction(nameof(GetById), new { id = newsArticle.NewsArticleId }, NewsArticleMapper.ToDto(newsArticle));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrStaff")]
    public IActionResult Update(string id, [FromBody] UpdateNewsArticleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var newsArticle = new NewsArticle
        {
            NewsArticleId = id,
            NewsTitle = dto.NewsTitle ?? string.Empty,
            Headline = dto.Headline ?? string.Empty,
            NewsContent = dto.NewsContent,
            NewsSource = dto.NewsSource,
            CategoryId = dto.CategoryId,
            NewsStatus = dto.NewsStatus,
            CreatedById = dto.CreatedById,
            // Tags will be set below
        };
        if (dto.TagIds != null && dto.TagIds.Count > 0)
        {
            foreach (var tagId in dto.TagIds.Distinct())
            {
                var tag = new BussinessObject.Models.Tag { TagId = tagId };
                newsArticle.Tags.Add(tag);
            }
        }
        _service.Update(newsArticle);
        var updated = _service.GetById(id);
        if (updated == null) return NotFound();
        return Ok(NewsArticleMapper.ToDto(updated));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOrStaff")]
    public IActionResult Delete(string id)
    {
        _service.Delete(id);
        return NoContent();
    }

    [HttpGet("mine")]
    [Authorize(Policy = "AdminOrStaff")]
    public IActionResult GetMyArticles()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => string.Equals(c.Type, "AccountId", StringComparison.OrdinalIgnoreCase) || c.Type.EndsWith("/accountId", StringComparison.OrdinalIgnoreCase));
        if (userIdClaim == null)
            return Unauthorized("No accountId claim found in token.");
        if (!short.TryParse(userIdClaim.Value, out var userId))
            return BadRequest("Invalid accountId in token.");
        var articles = _service.GetAll()
            .Where(n => n.CreatedById == userId)
            .Select(NewsArticleMapper.ToDto)
            .ToList();
        return Ok(articles);
    }

    [HttpGet("report")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult GetReportByPeriod([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate)
    {
        if (endDate < startDate)
            return BadRequest("EndDate must be after StartDate.");
        var articles = _service.GetByPeriod(startDate, endDate)
            .Where(n => n.NewsStatus == true)
            .Select(NewsArticleMapper.ToDto)
            .ToList();
        return Ok(articles);
    }
}
