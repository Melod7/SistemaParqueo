namespace SistemaParqueo.MVC.Models
{
    public class VehiculoIngresoViewModel
    {
        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string TipoTarifa { get; set; } = string.Empty;
        public DateTime FechaIngreso { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public VehiculoSalidaViewModel? Salida { get; set; }

    }
}