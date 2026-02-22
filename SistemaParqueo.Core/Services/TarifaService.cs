using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaParqueo.Core.Services
{
    public class TarifaService
    {
        public decimal CalcularMonto(string tipoTarifa, int horas)
        {
            if (horas > 5)
            {
                return tipoTarifa == "VIP" ? 8 : 5;
            }

            if (tipoTarifa == "VIP")
                return horas * 2;

            return horas * 1;
        }
    }
}