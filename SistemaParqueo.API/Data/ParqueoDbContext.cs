using Microsoft.EntityFrameworkCore;
using SistemaParqueo.Core.Entities;

namespace SistemaParqueo.API.Data;

public class ParqueoDbContext : DbContext
{
    public ParqueoDbContext(DbContextOptions<ParqueoDbContext> options)
        : base(options)
    {
    }

    public DbSet<VehiculoIngreso> Ingresos { get; set; }
    public DbSet<VehiculoSalida> Salidas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehiculoIngreso>()
            .HasOne(v => v.Salida)
            .WithOne(s => s.Ingreso)
            .HasForeignKey<VehiculoSalida>(s => s.IngresoId);
    }
}