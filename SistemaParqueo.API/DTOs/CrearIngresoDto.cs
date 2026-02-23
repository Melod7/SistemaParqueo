namespace SistemaParqueo.API.DTOs;

public class CrearIngresoDto
{
    public string Placa { get; set; } = string.Empty;
    public string TipoTarifa { get; set; } = string.Empty;
    public string? Usuario { get; set; }
    public string? Observaciones { get; set; }
}