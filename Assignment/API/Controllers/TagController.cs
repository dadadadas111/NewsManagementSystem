using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service;
using BussinessObject.Models;
using API.DTOs;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly TagService _service = new();
    private readonly ILogger<TagController> _logger;
    public TagController(ILogger<TagController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    [EnableQuery]
    public IQueryable<TagDto> GetAll()
    {
        _logger.LogInformation("TagController.GetAll called");
        return _service.GetAll().Select(TagMapper.ToDto).AsQueryable();
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IActionResult GetById(int id)
    {
        _logger.LogInformation($"TagController.GetById called with id={id}");
        var item = _service.GetById(id);
        if (item == null) return NotFound();
        return Ok(TagMapper.ToDto(item));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrStaff")]
    public IActionResult Add([FromBody] CreateTagDto dto)
    {
        _logger.LogInformation($"TagController.Add called with TagName={dto.TagName}");
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        // Duplicate name check
        if (_service.GetAll().Any(t => t.TagName == dto.TagName))
            return BadRequest("A tag with this name already exists.");
        var tag = new Tag
        {
            TagId = dto.TagId ?? 0, // Use supplied TagId if present, else 0 (auto-increment if not supplied)
            TagName = dto.TagName,
            Note = dto.Note
        };
        _service.Add(tag);
        return CreatedAtAction(nameof(GetById), new { id = tag.TagId }, tag);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrStaff")]
    public IActionResult Update(int id, [FromBody] UpdateTagDto dto)
    {
        _logger.LogInformation($"TagController.Update called with id={id}, TagName={dto.TagName}");
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var tag = new Tag
        {
            TagId = id,
            TagName = dto.TagName,
            Note = dto.Note
        };
        _service.Update(tag);
        var updated = _service.GetById(id);
        if (updated == null) return NotFound();
        return Ok(TagMapper.ToDto(updated));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOrStaff")]
    public IActionResult Delete(int id)
    {
        _logger.LogInformation($"TagController.Delete called with id={id}");
        try
        {
            _service.Delete(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return NoContent();
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] string keyword)
        => Ok(_service.Search(keyword));
}
