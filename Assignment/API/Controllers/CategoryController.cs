using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service;
using BussinessObject.Models;
using API.DTOs;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _service = new();

    [HttpGet]
    [AllowAnonymous]
    [EnableQuery]
    public IQueryable<CategoryDto> GetAll()
    {
        return _service.GetAll().Select(CategoryMapper.ToDto).AsQueryable();
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IActionResult GetById(short id)
    {
        var item = _service.GetById(id);
        if (item == null) return NotFound();
        return Ok(CategoryMapper.ToDto(item));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrStaff")]
    public IActionResult Add([FromBody] CreateCategoryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var category = new Category
        {
            CategoryName = dto.CategoryName,
            CategoryDesciption = dto.CategoryDescription ?? string.Empty,
            ParentCategoryId = dto.ParentCategoryId,
            IsActive = dto.IsActive ?? true
        };
        _service.Add(category);
        return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrStaff")]
    public IActionResult Update(short id, [FromBody] UpdateCategoryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var category = new Category
        {
            CategoryId = id,
            CategoryName = dto.CategoryName ?? string.Empty,
            CategoryDesciption = dto.CategoryDescription ?? string.Empty
        };
        _service.Update(category);
        var updated = _service.GetById(id);
        if (updated == null) return NotFound();
        return Ok(CategoryMapper.ToDto(updated));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOrStaff")]
    public IActionResult Delete(short id)
    {
        var category = _service.GetById(id);
        if (category != null && category.NewsArticles != null && category.NewsArticles.Count > 0)
        {
            return BadRequest("Cannot delete category: it is used by one or more news articles.");
        }
        _service.Delete(id);
        return NoContent();
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] string keyword)
        => Ok(_service.Search(keyword));
}
