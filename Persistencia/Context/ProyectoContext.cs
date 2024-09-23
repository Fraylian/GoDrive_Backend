using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistencia.Context
{
    public class ProyectoContext: IdentityDbContext<Usuarios>
    {
        public ProyectoContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Clientes> clientes { get; set; }
        public DbSet<factura> factura { get; set; }
        public DbSet<factura_detalle> factura_Detalles { get; set; }
        public DbSet<Usuarios> usuarios { get; set; }
        public DbSet<Vehiculos> vehiculos { get; set; }
    }
}
