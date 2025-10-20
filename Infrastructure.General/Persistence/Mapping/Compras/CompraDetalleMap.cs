namespace Infrastructure.General.Persistence.Mapping.Compras
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CompraDetalleMap : IEntityTypeConfiguration<CompraDetalle>
    {
        #region Métodos

        /// <summary>
        /// Método para realizar la configuración de la entidad según la BD.
        /// </summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<CompraDetalle> builder)
        {
            builder.ToTable("CompraDetalle", "Compras", t => t.HasComment("Contiene los detalles individuales de cada compra de vuelo."));

            builder.HasKey(e => e.CompraDetalleId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.CompraDetalleId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del detalle de la compra.");

            builder.Property(e => e.CompraId)
                .IsRequired()
                .HasComment("Identificador de la compra asociada.");

            builder.Property(e => e.VueloAsientoId)
                .IsRequired()
                .HasComment("Identificador del asiento del vuelo adquirido.");

            builder.Property(e => e.CompraDetalleNombrePasajero)
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("Nombre del pasajero.");

            builder.Property(e => e.CompraDetalleIdentificacionPasajero)
                .HasMaxLength(50)
                .IsRequired()
                .HasComment("Identificación del pasajero.");

            builder.Property(e => e.CompraDetallePrecio)
                .HasColumnType("decimal(18,2)")
                .IsRequired()
                .HasComment("Precio del asiento o pasaje.");

            builder.HasOne(cd => cd.VueloAsiento)
    .WithOne(va => va.CompraDetalle)
    .HasForeignKey<CompraDetalle>(cd => cd.VueloAsientoId)
    .OnDelete(DeleteBehavior.Restrict)
    .HasConstraintName("FK_CompraDetalle_VueloAsiento_VueloAsientoId");
        }

        #endregion
    }
}
