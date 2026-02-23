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
            .SumAsync(s => s.MontoCobrado > 0 ? s.MontoCobrado : 0);

        return Ok(total);
    }

    [HttpGet("recaudado-rango")]
    public async Task<IActionResult> TotalPorRango(DateTime inicio, DateTime fin)
    {
        var inicioUtc = DateTime.SpecifyKind(inicio.Date, DateTimeKind.Utc);
        var finUtc = DateTime.SpecifyKind(fin.Date.AddDays(1), DateTimeKind.Utc);

        var total = await _context.Salidas
            .Where(s => s.FechaSalida >= inicioUtc && s.FechaSalida < finUtc)
            .SumAsync(s => (decimal?)s.MontoCobrado) ?? 0;

        return Ok(total);
    }

    [HttpGet("detalle-rango")]
    public async Task<IActionResult> DetallePorRango(DateTime inicio, DateTime fin)
    {
        var inicioUtc = DateTime.SpecifyKind(inicio.Date, DateTimeKind.Utc);
        var finUtc = DateTime.SpecifyKind(fin.Date.AddDays(1), DateTimeKind.Utc);

        var reporte = await _context.Salidas
            .Include(s => s.Ingreso)
            .Where(s => s.FechaSalida >= inicioUtc && s.FechaSalida < finUtc)
            .Select(s => new
            {
                placa = s.Ingreso.Placa,
                tarifa = s.Ingreso.TipoTarifa,
                fechaIngreso = s.Ingreso.FechaIngreso,
                fechaSalida = s.FechaSalida,
                horas = s.HorasCalculadas,
                monto = s.MontoCobrado
            })
            .OrderByDescending(x => x.fechaSalida)
            .ToListAsync();

        return Ok(reporte);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var ultimos = await _context.Ingresos
            .OrderByDescending(i => i.FechaIngreso)
            .Take(10)
            .Select(i => new
            {
                i.Placa,
                i.TipoTarifa,
                i.FechaIngreso,
                FechaSalida = i.Salida != null ? i.Salida.FechaSalida : (DateTime?)null,
                Monto = i.Salida != null ? i.Salida.MontoCobrado : 0
            })
            .ToListAsync();

        return Ok(ultimos);
    
    }
    [HttpGet("dashboard-metricas")]
    public async Task<IActionResult> DashboardMetricas()
    {
        var hoyInicio = DateTime.UtcNow.Date;
        var hoyFin = hoyInicio.AddDays(1);

        var activos = await _context.Ingresos
            .CountAsync(i => i.Activo);

        var ingresosHoy = await _context.Ingresos
            .CountAsync(i => i.FechaIngreso >= hoyInicio && i.FechaIngreso < hoyFin);

        var salidasHoy = await _context.Salidas
            .CountAsync(s => s.FechaSalida >= hoyInicio && s.FechaSalida < hoyFin);

        var recaudadoHoy = await _context.Salidas
            .Where(s => s.FechaSalida >= hoyInicio && s.FechaSalida < hoyFin)
            .SumAsync(s => (decimal?)s.MontoCobrado) ?? 0;

        return Ok(new
        {
            activos,
            ingresosHoy,
            salidasHoy,
            recaudadoHoy
        });
    }
}