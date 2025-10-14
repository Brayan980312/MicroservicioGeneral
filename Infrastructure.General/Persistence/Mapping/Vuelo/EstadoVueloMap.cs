namespace Infrastructure.General.Persistence.Mapping.Vuelo
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>Configuración de la entidad EstadoVuelo para su mapeo con la base de datos.</summary>
    public class EstadoVueloMap : IEntityTypeConfiguration<EstadoVuelo>
    {
        #region Métodos

        /// <summary>Método para realizar la configuración de la entidad según la BD.</summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<EstadoVuelo> builder)
        {
            builder.ToTable("EstadoVuelo", "Vuelo");

            builder.HasComment("Almacena la información de los estados que puede tener un vuelo, como Programado, En vuelo, Cancelado, etc.");

            builder.HasKey(e => e.EstadoVueloId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.EstadoVueloId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del estado de vuelo.");

            builder.Property(e => e.EstadoVueloNombre)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre del estado de vuelo (por ejemplo, 'Programado', 'En vuelo', 'Cancelado').");

            builder.Property(e => e.EstadoVueloDescripcion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("Descripción detallada del estado de vuelo.");

            builder.Property(e => e.EstadoVueloEstado)
                .IsRequired()
                .HasComment("Indica si el estado de vuelo está activo o inactivo. Valor 1 = Activo, 0 = Inactivo.");
        }

        #endregion
    }
}