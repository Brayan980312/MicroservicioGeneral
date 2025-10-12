namespace Infrastructure.General.Persistence.Mapping.Avion
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Configuración de la entidad Avion para su mapeo con la base de datos.
    /// </summary>
    public class AvionMap : IEntityTypeConfiguration<Avion>
    {
        #region Métodos

        /// <summary>Método para realizar la configuración de la entidad según la BD.</summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<Avion> builder)
        {
            builder.ToTable("Avion", "Avion");

            builder.HasComment("Almacena la información de los aviones registrados en el sistema, incluyendo su ciudad base y estado activo/inactivo.");

            builder.HasKey(e => e.AvionId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.AvionId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del avión.");

            builder.Property(e => e.AvionNombre)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre del avión.");

            builder.Property(e => e.CiudadId)
                .IsRequired()
                .HasComment("Identificador de la ciudad en donde se encuentra el avión.");

            builder.Property(e => e.AvionEstado)
                .IsRequired()
                .HasComment("Estado del avión. Valor 1 = Activo, 0 = Inactivo.");

            builder.HasOne<Ciudad>()
                .WithMany()
                .HasForeignKey(e => e.CiudadId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Avion_Ciudad");
        }

        #endregion
    }
}
