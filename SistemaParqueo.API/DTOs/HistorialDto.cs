namespace SistemaParqueo.API.DTOs
{
    public class HistorialDto
    {
        public string Placa { get; set; } = string.Empty;
        public string TipoTarifa { get; set; } = string.Empty;
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaSalida { get; set; }
        public int Horas { get; set; }
        public decimal Monto { get; set; }
    }
}