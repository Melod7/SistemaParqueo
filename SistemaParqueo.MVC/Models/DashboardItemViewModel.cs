namespace SistemaParqueo.MVC.Models
{
    public class DashboardItemViewModel
    {
        public string Placa { get; set; } = string.Empty;
        public string TipoTarifa { get; set; } = string.Empty;
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaSalida { get; set; }
        public decimal Monto { get; set; }
    }
}