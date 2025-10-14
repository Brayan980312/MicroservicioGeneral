namespace Infrastructure.General.Persistence.Mapping.Vuelo
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CompraDetalleHistoricoMap : IEntityTypeConfiguration<CompraDetalleHistorico>
    {
        #region Métodos

        /// <summary>
        /// Método para realizar la configuración de la entidad según la BD.
        /// </summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<CompraDetalleHistorico> builder)
        {
            builder.ToTable("CompraDetalleHistorico", "Vuelo");

            builder.HasComment("Contiene el historial de los detalles de compra de vuelos.");

            builder.HasKey(e => e.CompraDetalleHistoricoId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.CompraDetalleHistoricoId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del registro histórico del detalle de compra.");

            builder.Property(e => e.CompraDetalleId)
                .IsRequired()
                .HasComment("Identificador del detalle de compra asociado.");

            builder.Property(e => e.CompraDetalleHistoricoNombrePasajero)
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("Nombre del pasajero en el historial.");

            builder.Property(e => e.CompraDetalleHistoricoIdentificacionPasajero)
                .HasMaxLength(50)
                .IsRequired()
                .HasComment("Identificación del pasajero en el historial.");

            builder.Property(e => e.CompraDetalleHistoricoFechaRegistro)
                .IsRequired()
                .HasComment("Fecha de registro del historial.");
        }

        #endregion
    }
}
