using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service;
using BussinessObject.Models;
using API.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly TagService _service = new();

    /// <remarks>
    /// <b>OData Query Options Supported:</b><br/>
    /// <ul>
    /// <li><code>$orderby</code> (e.g. <code>?$orderby=TagName</code>)</li>
    /// <li><code>$top</code> (e.g. <code>?$top=10</code>)</li>
    /// <li><code>$skip</code> (e.g. <code>?$skip=10</code>)</li>
    /// <li><code>$filter</code> (e.g. <code>?$filter=TagId gt 1</code>)</li>
    /// </ul>
    /// </remarks>
    /// <summary>
    /// Gets all tags. Supports OData query options: $orderby, $top, $skip, $filter.
    /// Example: /api/Tag?$orderby=TagName&$top=10
    /// </summary>
    /// <returns>Queryable list of Tag</returns>
    [HttpGet]
    [EnableQuery]
    public IQueryable<TagDto> GetAll()
    {
        return _service.GetAll().Select(TagMapper.ToDto).AsQueryable();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var item = _service.GetById(id);
        if (item == null) return NotFound();
        return Ok(TagMapper.ToDto(item));
    }

    [HttpPost]
    public IActionResult Add(Tag tag)
    {
        _service.Add(tag);
        return CreatedAtAction(nameof(GetById), new { id = tag.TagId }, tag);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Tag tag)
    {
        if (id != tag.TagId) return BadRequest();
        _service.Update(tag);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return NoContent();
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] string keyword)
        => Ok(_service.Search(keyword));
}
