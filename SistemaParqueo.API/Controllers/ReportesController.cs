using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaParqueo.API.Data;

namespace SistemaParqueo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportesController : ControllerBase
{
    private readonly ParqueoDbContext _context;

    public ReportesController(ParqueoDbContext context)
    {
        _context = context;
    }

    // Total recaudado por día
    [HttpGet("recaudado-dia")]
    public async Task<IActionResult> TotalPorDia(DateTime fecha)
    {
        var fechaInicio = fecha.Date;
        var fechaFin = fechaInicio.AddDays(1);

        var total = await _context.Salidas
            .Where(s => s.FechaSalida >= fechaInicio && s.FechaSalida < fechaFin)
            .SumAsync(s => (decimal?)s.MontoCobrado) ?? 0;

        return Ok(total);
    }

    [HttpGet("recaudado-rango")]
    public async Task<IActionResult> TotalPorRango(DateTime inicio, DateTime fin)
    {
        var fechaInicio = DateTime.SpecifyKind(inicio.Date, DateTimeKind.Utc);
        var fechaFin = DateTime.SpecifyKind(fin.Date.AddDays(1), DateTimeKind.Utc);

        var total = await _context.Salidas
            .Where(s => s.FechaSalida >= fechaInicio && s.FechaSalida < fechaFin)
            .SumAsync(s => (decimal?)s.MontoCobrado) ?? 0;

        return Ok(total);
    }
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var ultimos = await _context.Ingresos
            .Include(i => i.Salida)
            .OrderByDescending(i => i.FechaIngreso)
            .Take(10)
            .Select(i => new
            {
                i.Placa,
                i.TipoTarifa,
                i.FechaIngreso,
                FechaSalida = i.Salida != null ? (DateTime?)i.Salida.FechaSalida : null,
                Monto = i.Salida != null ? i.Salida.MontoCobrado : 0
            })
            .ToListAsync();

        return Ok(ultimos);
    }
}