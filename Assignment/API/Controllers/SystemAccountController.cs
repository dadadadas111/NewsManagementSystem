using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service;
using BussinessObject.Models;
using API.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SystemAccountController : ControllerBase
{
    private readonly SystemAccountService _service = new();

    [HttpGet]
    [EnableQuery]
    public IQueryable<SystemAccountDto> GetAll()
    {
        return _service.GetAll().Select(SystemAccountMapper.ToDto).AsQueryable();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(short id)
    {
        var item = _service.GetById(id);
        if (item == null) return NotFound();
        return Ok(SystemAccountMapper.ToDto(item));
    }

    [HttpPost]
    public IActionResult Add(SystemAccount account)
    {
        _service.Add(account);
        return CreatedAtAction(nameof(GetById), new { id = account.AccountId }, account);
    }

    [HttpPut("{id}")]
    public IActionResult Update(short id, SystemAccount account)
    {
        if (id != account.AccountId) return BadRequest();
        _service.Update(account);
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
