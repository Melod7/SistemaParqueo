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
        if (string.IsNullOrEmpty(dto.Placa))
            return BadRequest("La placa es obligatoria");

        if (!new[] { "VIP", "ESTANDAR" }.Contains(dto.TipoTarifa.ToUpper()))
            return BadRequest("Tipo de tarifa inválido");

        var existeActivo = _context.Ingresos
            .Any(i => i.Placa == dto.Placa && i.Activo);

        if (existeActivo)
            return BadRequest("El vehículo ya se encuentra dentro del parqueadero");

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
            .Select(v => new
            {
                v.Placa,
                v.TipoTarifa,
                v.FechaIngreso
            })
            .ToListAsync();

        return Ok(activos);
    }

    // Historial por placa
    [HttpGet("historial/{placa}")]
    public async Task<IActionResult> Historial(string placa)
    {
        var historial = await _context.Ingresos
            .Include(i => i.Salida)
            .Where(i => i.Placa == placa)
            .Select(i => new HistorialDto
            {
                Placa = i.Placa,
                TipoTarifa = i.TipoTarifa,
                FechaIngreso = i.FechaIngreso,
                FechaSalida = i.Salida != null ? i.Salida.FechaSalida : null,
                Horas = i.Salida != null ? i.Salida.HorasCalculadas : 0,
                Monto = i.Salida != null ? i.Salida.MontoCobrado : 0
            })
            .ToListAsync();

        if (!historial.Any())
            return NotFound("No existe historial");

        return Ok(historial);
    }
}