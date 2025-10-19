using Domain.General.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Configuración de la entidad VuelosMasComprados para su mapeo con la base de datos.
/// </summary>
public class VuelosMasCompradosMap : IEntityTypeConfiguration<VuelosMasComprados>
{
    public void Configure(EntityTypeBuilder<VuelosMasComprados> builder)
    {
        builder.ToTable("VuelosMasComprados", "Metricas", t =>
            t.HasComment("Registra las métricas de los vuelos más comprados en el sistema."));

        builder.HasKey(e => e.VuelosMasCompradosId);

        builder.Ignore(e => e.Id);

        builder.Property(e => e.VuelosMasCompradosId)
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único del registro de vuelos más comprados.");

        builder.Property(e => e.CiudadOrigenId)
            .IsRequired()
            .HasComment("Identificador de la ciudad de origen.");

        builder.Property(e => e.CiudadDestinoId)
            .IsRequired()
            .HasComment("Identificador de la ciudad de destino.");

        builder.Property(e => e.VuelosMasCompradosCantidadActual)
            .IsRequired()
            .HasDefaultValue(0)
            .HasComment("Cantidad total de veces que se ha comprado este vuelo.");

        builder.HasOne(e => e.CiudadOrigen)
            .WithMany()
            .HasForeignKey(e => e.CiudadOrigenId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_VuelosMasComprados_CiudadOrigen_CiudadId");

        builder.HasOne(e => e.CiudadDestino)
            .WithMany()
            .HasForeignKey(e => e.CiudadDestinoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_VuelosMasComprados_CiudadDestino_CiudadId");
    }
}