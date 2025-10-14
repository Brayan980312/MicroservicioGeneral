namespace Infrastructure.General.Persistence.Mapping.Vuelo
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class VueloHistoricoMap : IEntityTypeConfiguration<VueloHistorico>
    {
        #region Métodos

        /// <summary>
        /// Método para realizar la configuración de la entidad según la BD.
        /// </summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<VueloHistorico> builder)
        {
            builder.ToTable("VueloHistorico", "Vuelo");

            builder.HasComment("Contiene el historial de cambios de información de los vuelos.");

            builder.HasKey(e => e.VueloHistoricoId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.VueloHistoricoId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del registro histórico del vuelo.");

            builder.Property(e => e.VueloId)
                .IsRequired()
                .HasComment("Identificador del vuelo asociado.");

            builder.Property(e => e.VueloHistoricoCodigo)
                .HasMaxLength(50)
                .IsRequired()
                .HasComment("Código histórico del vuelo.");

            builder.Property(e => e.EstadoVueloId)
                .IsRequired()
                .HasComment("Identificador del estado del vuelo en el historial.");

            builder.Property(e => e.AvionId)
                    .IsRequired()
                    .HasComment("Identificador del avión asociado al vuelo.");

            builder.Property(e => e.CiudadOrigenId)
                .IsRequired()
                .HasComment("Identificador de la ciudad de origen.");

            builder.Property(e => e.CiudadDestinoId)
                .IsRequired()
                .HasComment("Identificador de la ciudad de destino.");

            builder.Property(e => e.VueloHistoricoPrecio)
                .HasColumnType("decimal(18,2)")
                .IsRequired()
                .HasComment("Precio del vuelo en el historial.");

            builder.Property(e => e.VueloHistoricoDescuento)
                .HasColumnType("decimal(18,2)")
                .IsRequired()
                .HasComment("Descuento aplicado en el historial.");

            builder.Property(e => e.VueloHistoricoFechaHoraSalida)
                .IsRequired()
                .HasComment("Fecha y hora de salida en el historial.");

            builder.Property(e => e.VueloHistoricoFechaHoraLlegada)
                .IsRequired()
                .HasComment("Fecha y hora de llegada en el historial.");

            builder.Property(e => e.UsuarioCreacionId)
                .IsRequired()
                .HasComment("Identificador del usuario que registró el historial.");

            builder.Property(e => e.VueloHistoricoFechaCreacion)
                .IsRequired()
                .HasComment("Fecha de creación del registro histórico.");
        }

        #endregion
    }
}
