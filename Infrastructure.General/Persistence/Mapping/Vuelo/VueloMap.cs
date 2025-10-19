namespace Infrastructure.General.Persistence.Mapping.Vuelo
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class VueloMap : IEntityTypeConfiguration<Vuelo>
    {
        #region Métodos

        /// <summary>
        /// Método para realizar la configuración de la entidad según la BD.
        /// </summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<Vuelo> builder)
        {
            builder.ToTable("Vuelo", "Vuelo", t => t.HasComment("Almacena la información de los vuelos registrados en el sistema."));

            builder.HasKey(e => e.VueloId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.VueloId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del vuelo.");

            builder.Property(e => e.VueloCodigo)
                .HasMaxLength(50)
                .IsRequired()
                .HasComment("Código del vuelo.");

            builder.Property(e => e.EstadoVueloId)
                .IsRequired()
                .HasComment("Identificador del estado del vuelo.");

            builder.Property(e => e.AvionId)
                .IsRequired()
                .HasComment("Identificador del avión asociado al vuelo.");

            builder.Property(e => e.CiudadOrigenId)
                .IsRequired()
                .HasComment("Identificador de la ciudad de origen.");

            builder.Property(e => e.CiudadDestinoId)
                .IsRequired()
                .HasComment("Identificador de la ciudad de destino.");

            builder.Property(e => e.VueloPrecio)
                .HasColumnType("decimal(18,2)")
                .IsRequired()
                .HasComment("Precio del vuelo.");

            builder.Property(e => e.VueloDescuento)
                .HasColumnType("decimal(18,2)")
                .IsRequired()
                .HasComment("Descuento aplicado al vuelo.");

            builder.Property(e => e.VueloFechaHoraSalida)
                .IsRequired()
                .HasComment("Fecha y hora de salida del vuelo.");

            builder.Property(e => e.VueloFechaHoraLlegada)
                .IsRequired()
                .HasComment("Fecha y hora de llegada del vuelo.");

            builder.Property(e => e.UsuarioCreacionId)
                .IsRequired()
                .HasComment("Identificador del usuario que creó el vuelo.");

            builder.Property(e => e.VueloFechaCreacion)
                .IsRequired()
                .HasComment("Fecha de creación del vuelo.");

            builder.HasOne(e => e.EstadoVuelo)
                .WithMany()
                .HasForeignKey(e => e.EstadoVueloId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Vuelo_EstadoVuelo_EstadoVueloId");

            builder.HasOne(e => e.Avion)
                .WithMany()
                .HasForeignKey(e => e.AvionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Vuelo_Avion_AvionId");

            builder.HasOne(e => e.CiudadOrigen)
                .WithMany()
                .HasForeignKey(e => e.CiudadOrigenId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Vuelo_CiudadOrigen_CiudadOrigenId");

            builder.HasOne(e => e.CiudadDestino)
                .WithMany()
                .HasForeignKey(e => e.CiudadDestinoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Vuelo_CiudadDestino_CiudadDestinoId");
        }

        #endregion
    }
}
