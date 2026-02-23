using System;
using System.Collections.Generic;
using System.Text;


namespace SistemaParqueo.Core.Entities
{
    public class VehiculoSalida
    {
        public int Id { get; set; }
        public int IngresoId { get; set; }
        public DateTime FechaSalida { get; set; }
        public int HorasCalculadas { get; set; }
        public decimal MontoCobrado { get; set; }
        public string? MetodoPago { get; set; }
        public VehiculoIngreso? Ingreso { get; set; }

    }
}