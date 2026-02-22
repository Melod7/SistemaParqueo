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

    [HttpGet("recaudado-dia")]
    public async Task<IActionResult> TotalPorDia(DateTime fecha)
    {
        var fechaUtc = DateTime.SpecifyKind(fecha, DateTimeKind.Utc);

        var total = await _context.Salidas
            .Where(x => x.FechaSalida.Date == fechaUtc.Date)
            .SumAsync(x => x.MontoCobrado);

        return Ok(total);
    }
}