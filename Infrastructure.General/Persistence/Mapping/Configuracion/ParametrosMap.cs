namespace Infrastructure.General.Persistence.Mapping.Configuracion
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ParametrosMap : IEntityTypeConfiguration<Parametros>
    {
        #region Métodos

        /// <summary>Método para realizar la configuración de la entidad según la BD.</summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<Parametros> builder)
        {
            builder.ToTable("Parametros", "Configuracion");

            builder.HasComment("Almacena los parámetros configurables de la aplicación, como valores generales o de comportamiento del sistema.");

            builder.HasKey(e => e.ParametrosId);

            builder.Ignore(e => e.Id); // Solo si tu entidad base tiene una propiedad genérica 'Id'

            builder.Property(e => e.ParametrosId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del parámetro de aplicación.");

            builder.Property(e => e.ParametrosNombre)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre del parámetro de configuración.");

            builder.Property(e => e.ParametrosValor)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Valor asignado al parámetro de configuración.");

            builder.Property(e => e.ParametrosDescripcion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("Descripción o detalle del propósito del parámetro.");

            builder.Property(e => e.ParametrosEstado)
                .IsRequired()
                .HasComment("Indica si el parámetro está activo (1) o inactivo (0).");
        }

        #endregion
    }
}
