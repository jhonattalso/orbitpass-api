using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace OrbitPass.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
        => _configuration = configuration;

    /// <summary>Gera um token JWT para testes.</summary>
    [AllowAnonymous]
    [HttpPost("token")]
    public IActionResult GerarToken([FromBody] LoginRequest request) {
        // Validação simplificada para fins acadêmicos
        // Em produção: validar contra banco de dados
        if (request.Email != "teste@orbitpass.com" || request.Senha != "Senha@123")
            return Unauthorized(new { mensagem = "Credenciais inválidas." });

        var token = CriarToken(request.Email);
        return Ok(new { token });
    }

    private string CriarToken(string email) {
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record LoginRequest(string Email, string Senha);