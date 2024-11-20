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
            //Configuracion para imagenes
            base.OnModelCreating(builder);
            builder.Entity<Imagen>().HasOne(v => v.Vehiculo).WithMany(v => v.imagenes).HasForeignKey(v => v.vehiculo_id);

            //Configuracion para vehiculos
            builder.Entity<Vehiculos>().HasIndex(v => v.Matricula).IsUnique();
        }

        public DbSet<Clientes> clientes { get; set; }
        public DbSet<factura> factura { get; set; }
        public DbSet<factura_detalle> factura_Detalles { get; set; }
        public DbSet<Usuarios> usuarios { get; set; }
        public DbSet<Vehiculos> vehiculos { get; set; }
        public DbSet<Imagen> imagenes { get; set; }
    }
}
