using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaParqueo.API.Data;
using SistemaParqueo.Core.Entities;

namespace SistemaParqueo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalidasController : ControllerBase
{
    private readonly ParqueoDbContext _context;

    public SalidasController(ParqueoDbContext context)
    {
        _context = context;
    }

    [HttpPost("{placa}")]
    public async Task<IActionResult> RegistrarSalida(string placa)
    {
        var ingreso = await _context.Ingresos
            .FirstOrDefaultAsync(i => i.Placa == placa && i.Activo);

        if (ingreso == null)
            return NotFound("El vehículo no está dentro del parqueadero");

        var fechaSalida = DateTime.UtcNow;

        var horas = (DateTime.UtcNow - ingreso.FechaIngreso).TotalHours;
        var horasCalculadas = (int)Math.Ceiling(horas);

        decimal monto;

        if (ingreso.TipoTarifa == "VIP")
        {
            monto = horasCalculadas > 5 ? 8 : horasCalculadas * 2;
        }
        else
        {
            monto = horasCalculadas > 5 ? 5 : horasCalculadas * 1;
        }

        var salida = new VehiculoSalida
        {
            IngresoId = ingreso.Id,
            FechaSalida = fechaSalida,
            HorasCalculadas = horasCalculadas,
            MontoCobrado = monto,
            MetodoPago = "EFECTIVO"
        };

        ingreso.Activo = false;

        _context.Salidas.Add(salida);
        await _context.SaveChangesAsync();

        return Ok(salida);
    }
}