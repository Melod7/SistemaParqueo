namespace SistemaParqueo.MVC.Models
{
    public class VehiculoSalidaViewModel
    {
        public int Id { get; set; }
        public DateTime FechaSalida { get; set; }
        public int HorasCalculadas { get; set; }
        public decimal MontoCobrado { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
    }
}