namespace Infrastructure.General.Persistence.Mapping.Administracion
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PaisMap : IEntityTypeConfiguration<Pais>
    {
        #region Métodos

        /// <summary>Método para realizar la configuración de la entidad según la BD.</summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<Pais> builder)
        {
            builder.ToTable("Pais", "Administracion", t => t.HasComment("Almacena la información de los países registrados en el sistema, incluyendo su nomenclatura, si son internacionales y su estado activo/inactivo."));

            builder.HasKey(e => e.PaisId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.PaisId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del país.");

            builder.Property(e => e.PaisNombre)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre completo del país.");

            builder.Property(e => e.PaisNomenclatura)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Nomenclatura o código corto del país (por ejemplo, 'COL', 'USA').");

            builder.Property(e => e.PaisInternacional)
                .IsRequired()
                .HasComment("Indica si el país es considerado internacional. Valor 1 = Sí, 0 = No.");

            builder.Property(e => e.PaisEstado)
                .IsRequired()
                .HasComment("Estado del país. Valor 1 = Activo, 0 = Inactivo.");
        }

        #endregion
    }
}
