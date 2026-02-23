using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaParqueo.API.Data;

namespace SistemaParqueo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly ParqueoDbContext _context;

    public DashboardController(ParqueoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerResumen()
    {
        var vehiculosActivos = await _context.Ingresos
            .Where(i => i.Activo)
            .CountAsync();

        var totalHoy = await _context.Salidas
            .Where(s => s.FechaSalida.Date == DateTime.UtcNow.Date)
            .SumAsync(s => (decimal?)s.MontoCobrado) ?? 0;

        var ultimosIngresos = await _context.Ingresos
            .OrderByDescending(i => i.FechaIngreso)
            .Take(5)
            .ToListAsync();

        var ultimasSalidas = await _context.Salidas
            .OrderByDescending(s => s.FechaSalida)
            .Take(5)
            .ToListAsync();

        return Ok(new
        {
            vehiculosActivos,
            totalHoy,
            ultimosIngresos,
            ultimasSalidas
        });
    }
}