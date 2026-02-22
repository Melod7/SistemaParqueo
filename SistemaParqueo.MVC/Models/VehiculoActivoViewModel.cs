namespace SistemaParqueo.MVC.Models
{
    public class VehiculoActivoViewModel
    {
        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public DateTime FechaIngreso { get; set; }
        public string TipoTarifa { get; set; } = string.Empty;
    }
}