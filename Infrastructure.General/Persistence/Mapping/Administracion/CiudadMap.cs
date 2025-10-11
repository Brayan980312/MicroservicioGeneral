namespace Infrastructure.General.Persistence.Mapping.Administracion
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Configuración de la entidad Ciudad para su mapeo con la base de datos.
    /// </summary>
    public class CiudadMap : IEntityTypeConfiguration<Ciudad>
    {
        #region Métodos

        /// <summary>Método para realizar la configuración de la entidad según la BD.</summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<Ciudad> builder)
        {
            builder.ToTable("Ciudad", "Administracion");

            builder.HasComment("Almacena la información de las ciudades registradas en el sistema, incluyendo su nomenclatura, país asociado y estado activo/inactivo.");

            builder.HasKey(e => e.CiudadId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.CiudadId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único de la ciudad.");

            builder.Property(e => e.CiudadNombre)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre completo de la ciudad.");

            builder.Property(e => e.CiudadNomenclatura)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Nomenclatura o código corto de la ciudad (por ejemplo, 'BOG', 'MED').");

            builder.Property(e => e.PaisId)
                .IsRequired()
                .HasComment("Identificador del país al que pertenece la ciudad.");

            builder.Property(e => e.CiudadEstado)
                .IsRequired()
                .HasComment("Estado de la ciudad. Valor 1 = Activa, 0 = Inactiva.");
            
            builder.HasOne<Pais>()
                .WithMany()
                .HasForeignKey(e => e.PaisId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ciudad_Pais");
        }

        #endregion
    }
}
