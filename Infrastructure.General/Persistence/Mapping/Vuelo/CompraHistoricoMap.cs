namespace Infrastructure.General.Persistence.Mapping.Vuelo
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>Configuración de la entidad CompraHistorico para su mapeo con la base de datos.</summary>
    public class CompraHistoricoMap : IEntityTypeConfiguration<CompraHistorico>
    {
        #region Métodos

        /// <summary>Método para realizar la configuración de la entidad según la BD.</summary>
        /// <param name="builder">Entidad a configurar.</param>
        public void Configure(EntityTypeBuilder<CompraHistorico> builder)
        {
            builder.ToTable("CompraHistorico", "Vuelo");

            builder.HasComment("Registra el historial de cambios de estado de las compras realizadas en el sistema de vuelos.");

            builder.HasKey(e => e.CompraHistoricoId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.CompraHistoricoId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del registro histórico de la compra.");

            builder.Property(e => e.CompraId)
                .IsRequired()
                .HasComment("Identificador de la compra asociada.");

            builder.Property(e => e.EstadoCompraId)
                .IsRequired()
                .HasComment("Identificador del estado de compra asociado al registro histórico.");

            builder.Property(e => e.CompraHistoricoFechaRegistro)
                .IsRequired()
                .HasColumnType("datetime")
                .HasComment("Fecha y hora en que se registró el cambio de estado de la compra.");
        }

        #endregion
    }
}
