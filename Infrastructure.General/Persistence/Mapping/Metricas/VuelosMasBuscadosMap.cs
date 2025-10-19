namespace Infrastructure.General.Persistence.Mapping.Metricas
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Configuración de la entidad VuelosMasBuscados para su mapeo con la base de datos.
    /// </summary>
    public class VuelosMasBuscadosMap : IEntityTypeConfiguration<VuelosMasBuscados>
    {
        public void Configure(EntityTypeBuilder<VuelosMasBuscados> builder)
        {
            builder.ToTable("VuelosMasBuscados", "Metricas", t =>
                t.HasComment("Registra las métricas de los vuelos más buscados en el sistema."));

            builder.HasKey(e => e.VuelosMasBuscadosId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.VuelosMasBuscadosId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del registro de vuelos más buscados.");

            builder.Property(e => e.CiudadOrigenId)
                .IsRequired()
                .HasComment("Identificador de la ciudad de origen.");

            builder.Property(e => e.CiudadDestinoId)
                .IsRequired()
                .HasComment("Identificador de la ciudad de destino.");

            builder.Property(e => e.VuelosMasBuscadosCantidadActual)
                .IsRequired()
                .HasDefaultValue(0)
                .HasComment("Cantidad total de veces que se ha buscado este vuelo.");

            builder.HasOne(e => e.CiudadOrigen)
                .WithMany()
                .HasForeignKey(e => e.CiudadOrigenId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_VuelosMasBuscados_CiudadOrigen_CiudadId");

            builder.HasOne(e => e.CiudadDestino)
                .WithMany()
                .HasForeignKey(e => e.CiudadDestinoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_VuelosMasBuscados_CiudadDestino_CiudadId");
        }
    }
}