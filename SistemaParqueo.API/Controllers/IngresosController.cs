using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaParqueo.API.Data;
using SistemaParqueo.API.DTOs;
using SistemaParqueo.Core.Entities;

namespace SistemaParqueo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngresosController : ControllerBase
{
    private readonly ParqueoDbContext _context;

    public IngresosController(ParqueoDbContext context)
    {
        _context = context;
    }

    // Registrar ingreso
    [HttpPost]
    public async Task<IActionResult> RegistrarIngreso([FromBody] CrearIngresoDto dto)
    {
        var ingreso = new VehiculoIngreso
        {
            Placa = dto.Placa,
            TipoTarifa = dto.TipoTarifa,
            Usuario = dto.Usuario,
            Observaciones = dto.Observaciones,
            FechaIngreso = DateTime.UtcNow,
            Activo = true
        };

        _context.Ingresos.Add(ingreso);
        await _context.SaveChangesAsync();

        return Ok(ingreso);
    }

    // Vehículos actualmente dentro
    [HttpGet("activos")]
    public async Task<IActionResult> VehiculosActivos()
    {
        var activos = await _context.Ingresos
            .Where(v => v.Activo)
            .ToListAsync();

        return Ok(activos);
    }

    // Historial por placa
    [HttpGet("historial/{placa}")]
    public async Task<IActionResult> Historial(string placa)
    {
        var historial = await _context.Ingresos
            .Include(v => v.Salida)
            .Where(v => v.Placa == placa)
            .ToListAsync();

        if (_context.Ingresos.Count() > 0)
            return NotFound("No se encontró historial para esta placa");

        return Ok(historial);
    }
}