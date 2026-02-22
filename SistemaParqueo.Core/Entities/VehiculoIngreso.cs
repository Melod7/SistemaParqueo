using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaParqueo.Core.Entities
{
    public class VehiculoIngreso
    {
        public int Id { get; set; }
        public string Placa { get; set; }= string.Empty;
        public string TipoTarifa { get; set; }= string.Empty;
        public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
        public string? Usuario { get; set; }
        public string? Observaciones { get; set; }
        public bool Activo { get; set; }

        public VehiculoSalida? Salida { get; set; }
    }
}