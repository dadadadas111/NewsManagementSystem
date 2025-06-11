using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service;
using BussinessObject.Models;
using API.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _service = new();

    /// <remarks>
    /// <b>OData Query Options Supported:</b><br/>
    /// <ul>
    /// <li><code>$orderby</code> (e.g. <code>?$orderby=CategoryName</code>)</li>
    /// <li><code>$top</code> (e.g. <code>?$top=10</code>)</li>
    /// <li><code>$skip</code> (e.g. <code>?$skip=10</code>)</li>
    /// <li><code>$filter</code> (e.g. <code>?$filter=CategoryId gt 1</code>)</li>
    /// </ul>
    /// </remarks>
    /// <summary>
    /// Gets all categories. Supports OData query options: $orderby, $top, $skip, $filter.
    /// Example: /api/Category?$orderby=CategoryName&$top=10
    /// </summary>
    /// <returns>Queryable list of Category</returns>
    [HttpGet]
    [EnableQuery]
    public IQueryable<CategoryDto> GetAll()
    {
        return _service.GetAll().Select(CategoryMapper.ToDto).AsQueryable();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(short id)
    {
        var item = _service.GetById(id);
        if (item == null) return NotFound();
        return Ok(CategoryMapper.ToDto(item));
    }

    [HttpPost]
    public IActionResult Add(Category category)
    {
        _service.Add(category);
        return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
    }

    [HttpPut("{id}")]
    public IActionResult Update(short id, Category category)
    {
        if (id != category.CategoryId) return BadRequest();
        _service.Update(category);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(short id)
    {
        _service.Delete(id);
        return NoContent();
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] string keyword)
        => Ok(_service.Search(keyword));
}
