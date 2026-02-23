using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaParqueo.API.Data;
using SistemaParqueo.API.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ParqueoDbContext _context;

    public AuthController(ParqueoDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _context.Usuarios
            .FirstOrDefaultAsync(u =>
                u.Nombre == dto.Usuario &&
                u.Password == dto.Password);

        if (user == null)
            return Unauthorized("Credenciales incorrectas");

        return Ok(user);
    }
}