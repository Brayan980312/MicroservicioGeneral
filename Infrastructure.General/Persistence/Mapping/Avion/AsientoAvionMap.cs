namespace Infrastructure.General.Persistence.Mapping.Avion
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Configuración de la entidad AsientoAvion para su mapeo con la base de datos.
    /// </summary>
    public class AsientoAvionMap : IEntityTypeConfiguration<AsientoAvion>
    {
        #region Métodos

        /// <summary>Método para realizar la configuración de la entidad según la BD.</summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<AsientoAvion> builder)
        {
            builder.ToTable("AsientoAvion", "Avion");

            builder.HasComment("Almacena la información de los asientos pertenecientes a un avión, incluyendo si son VIP y su porcentaje adicional.");

            builder.HasKey(e => e.AsientoAvionId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.AsientoAvionId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del asiento.");

            builder.Property(e => e.AvionId)
                .IsRequired()
                .HasComment("Identificador del avión al que pertenece el asiento.");

            builder.Property(e => e.AsientoAvionNombre)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Nombre o código del asiento (por ejemplo, '1A', '2B').");

            builder.Property(e => e.AsientoAvionVIP)
                .IsRequired()
                .HasComment("Indica si el asiento es VIP. Valor 1 = Sí, 0 = No.");

            builder.Property(e => e.AsientoAvionVIPPorcentaje)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasComment("Porcentaje adicional aplicado a los asientos VIP.");

            builder.Property(e => e.AsientoAvionEstado)
                .IsRequired()
                .HasComment("Estado del asiento. Valor 1 = Activo, 0 = Inactivo.");

            builder.HasOne<Avion>()
                .WithMany()
                .HasForeignKey(e => e.AvionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_AsientoAvion_Avion");
        }

        #endregion
    }
}
