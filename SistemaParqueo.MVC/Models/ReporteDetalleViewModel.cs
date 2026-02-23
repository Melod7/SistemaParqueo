namespace SistemaParqueo.MVC.Models
{
    public class ReporteDetalleViewModel
    {
        public string Placa { get; set; }   = string.Empty;
        public string Tarifa { get; set; } = string.Empty;
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public int Horas { get; set; }
        public decimal Monto { get; set; }
    }
}
