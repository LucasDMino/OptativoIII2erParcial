using Microsoft.EntityFrameworkCore;

namespace OPTATIVOIII3ERPARCIAL.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cuentas> Cuentas { get; set; }
        public DbSet<Movimientos> Movimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ciudad>()
                .HasKey(c => c.idCiudad);

            modelBuilder.Entity<Persona>()
                .HasKey(p => p.idPersona);

            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.idCliente);

            modelBuilder.Entity<Cuentas>()
                .HasKey(c => c.idCuenta);

            modelBuilder.Entity<Movimientos>()
                .HasKey(m => m.idMovimiento);
        }
    }

}
