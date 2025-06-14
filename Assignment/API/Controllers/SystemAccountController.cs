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
public class SystemAccountController : ControllerBase
{
    private readonly SystemAccountService _service = new();
    private readonly ILogger<SystemAccountController> _logger;

    public SystemAccountController(ILogger<SystemAccountController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [EnableQuery]
    public IQueryable<SystemAccountDto> GetAll()
    {
        _logger.LogInformation("SystemAccountController.GetAll called");
        return _service.GetAll().Select(SystemAccountMapper.ToDto).AsQueryable();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(short id)
    {
        _logger.LogInformation($"SystemAccountController.GetById called with id={id}");
        var item = _service.GetById(id);
        if (item == null) return NotFound();
        return Ok(SystemAccountMapper.ToDto(item));
    }

    [HttpPost]
    public IActionResult Add([FromBody] CreateSystemAccountDto dto)
    {
        _logger.LogInformation($"SystemAccountController.Add called with AccountName={dto.AccountName}");
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var account = new SystemAccount
        {
            AccountId = dto.AccountId ?? 0, // Use supplied AccountId if present, else 0 (auto-increment if not supplied)
            AccountName = dto.AccountName,
            AccountEmail = dto.AccountEmail,
            AccountRole = dto.AccountRole,
            AccountPassword = dto.AccountPassword
        };
        _service.Add(account);
        return CreatedAtAction(nameof(GetById), new { id = account.AccountId }, account);
    }

    [HttpPut("{id}")]
    public IActionResult Update(short id, [FromBody] UpdateSystemAccountDto dto)
    {
        _logger.LogInformation($"SystemAccountController.Update called with id={id}, AccountName={dto.AccountName}");
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var account = new SystemAccount
        {
            AccountId = id,
            AccountName = dto.AccountName,
            AccountEmail = dto.AccountEmail,
            AccountRole = dto.AccountRole,
            AccountPassword = dto.AccountPassword
        };
        _service.Update(account);
        // Fetch updated entity and return as DTO
        var updated = _service.GetById(id);
        if (updated == null) return NotFound();
        return Ok(SystemAccountMapper.ToDto(updated));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(short id)
    {
        _logger.LogInformation($"SystemAccountController.Delete called with id={id}");
        _service.Delete(id);
        return NoContent();
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] string keyword)
        => Ok(_service.Search(keyword));
}
