using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaParqueo.API.Data;
using SistemaParqueo.Core.Entities;
using SistemaParqueo.Core.Services;

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
            .FirstOrDefaultAsync(x => x.Placa == placa && x.Activo);

        if (ingreso == null)
            return NotFound("Vehículo no encontrado o ya salió.");

        var salida = new VehiculoSalida();

        salida.IngresoId = ingreso.Id;
        salida.FechaSalida = DateTime.UtcNow;

        // calcular horas correctamente
        var horas = (salida.FechaSalida - ingreso.FechaIngreso).TotalHours;

        salida.HorasCalculadas = (int)Math.Ceiling(horas);

        // tarifas
        if (ingreso.TipoTarifa == "VIP")
        {
            salida.MontoCobrado = salida.HorasCalculadas > 5
                ? 8
                : salida.HorasCalculadas * 2;
        }
        else
        {
            salida.MontoCobrado = salida.HorasCalculadas > 5
                ? 5
                : salida.HorasCalculadas * 1;
        }

        ingreso.Activo = false;

        _context.Salidas.Add(salida);
        await _context.SaveChangesAsync();

        return Ok(salida);
    }
}