namespace SistemaParqueo.MVC.Models;

public class DashboardMetricasViewModel
{
    public int Activos { get; set; }
    public int IngresosHoy { get; set; }
    public int SalidasHoy { get; set; }
    public decimal RecaudadoHoy { get; set; }
}