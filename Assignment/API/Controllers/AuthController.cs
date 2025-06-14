using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.DTOs;
using BussinessObject.Models;
using Service;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly SystemAccountService _accountService = new();

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto dto)
    {
        // Check admin credentials from appsettings
        var adminSection = _config.GetSection("AdminAccount");
        var adminEmail = adminSection["Username"];
        var adminPass = adminSection["Password"];
        var adminRole = adminSection["Role"] ?? "Admin";
        if (dto.Username == adminEmail && dto.Password == adminPass)
        {
            var token = GenerateJwtToken(adminEmail, adminRole, null);
            return Ok(new LoginResponseDto { Token = token, Username = adminEmail, Role = adminRole });
        }
        // Check SystemAccount in DB
        var account = _accountService.GetAll().FirstOrDefault(a => a.AccountName == dto.Username || a.AccountEmail == dto.Username);
        if (account != null && account.AccountPassword == dto.Password)
        {
            string role = account.AccountRole == 1 ? "Staff" : "Lecturer"; // Assuming 1 is Staff, 2 is Lecturer
            var token = GenerateJwtToken(account.AccountEmail ?? account.AccountName ?? string.Empty, role, account.AccountId);
            return Ok(new LoginResponseDto { Token = token, Username = account.AccountEmail ?? account.AccountName ?? string.Empty, Role = role, AccountId = account.AccountId });
        }
        return Unauthorized("Invalid username or password");
    }

    private string GenerateJwtToken(string username, string role, short? accountId)
    {
        var jwtSection = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };
        if (accountId.HasValue)
            claims.Add(new Claim("AccountId", accountId.Value.ToString()));
        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSection["ExpireMinutes"]!)),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
