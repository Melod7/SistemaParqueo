using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaParqueo.MVC.Models;
using SistemaParqueo.MVC.Services;

namespace SistemaParqueo.MVC.Controllers;

public class ParqueoController : Controller
{
    private readonly ApiService _apiService;

    public ParqueoController(ApiService apiService)
    {
        _apiService = apiService;
    }

    // ================= PANEL =================

    public IActionResult Panel()
    {
        if (HttpContext.Session.GetString("Usuario") == null)
            return RedirectToAction("Login", "Account");

        return View();
    }

    // ================= DASHBOARD =================

    public async Task<IActionResult> Dashboard()
    {
        if (HttpContext.Session.GetString("Usuario") == null)
            return RedirectToAction("Login", "Account");

        var response = await _apiService.GetAsync("Reportes/dashboard");

        var datos = JsonConvert.DeserializeObject<List<DashboardItemViewModel>>(response);

        return View(datos);
    }

    // ================= INGRESO =================

    public IActionResult Ingreso()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Ingreso(VehiculoIngresoViewModel model)
    {
        var response = await _apiService.PostAsync("Ingresos", model);

        ViewBag.Mensaje = response.Contains("error")
            ? "Error al registrar vehículo"
            : "Vehículo registrado correctamente";

        return View();
    }

    // ================= SALIDA =================

    public IActionResult Salida()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Salida(string placa)
    {
        var response = await _apiService.PostAsync($"Salidas/{placa}", new { });

        ViewBag.Mensaje = "Salida registrada correctamente";
        ViewBag.Respuesta = response;

        return View();
    }

    // ================= ACTIVOS =================

    public async Task<IActionResult> Activos()
    {
        var response = await _apiService.GetAsync("Ingresos/activos");

        var lista = JsonConvert.DeserializeObject<List<VehiculoActivoViewModel>>(response);

        return View(lista);
    }

    // ================= HISTORIAL =================

    public async Task<IActionResult> Historial(string placa)
    {
        if (string.IsNullOrEmpty(placa))
            return View();

        var response = await _apiService.GetAsync($"Ingresos/historial/{placa}");

        var datos = JsonConvert.DeserializeObject<List<VehiculoIngresoViewModel>>(response);

        return View(datos);
    }

    // ================= REPORTE DIARIO =================

    public async Task<IActionResult> Reporte(DateTime fecha)
    {
        var response = await _apiService.GetAsync($"Reportes/recaudado-dia?fecha={fecha:yyyy-MM-dd}");

        ViewBag.Total = response;

        return View();
    }

    // ================= REPORTE RANGO =================

    public async Task<IActionResult> ReporteRango(DateTime inicio, DateTime fin)
    {
        var response = await _apiService.GetAsync(
            $"Reportes/recaudado-rango?inicio={inicio:yyyy-MM-dd}&fin={fin:yyyy-MM-dd}");

        ViewBag.Total = response;

        return View();
    }
    public async Task<IActionResult> ReporteDetalle(DateTime inicio, DateTime fin)
    {
        if (inicio == default || fin == default)
            return View();

        var response = await _apiService.GetAsync(
            $"Reportes/detalle-rango?inicio={inicio:yyyy-MM-dd}&fin={fin:yyyy-MM-dd}");

        var lista = JsonConvert.DeserializeObject<List<ReporteDetalleViewModel>>(response);

        return View(lista);
    }
}