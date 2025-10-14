namespace Infrastructure.General.Persistence.Mapping.Vuelo
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>Configuración de la entidad EstadoCompra para su mapeo con la base de datos.</summary>
    public class EstadoCompraMap : IEntityTypeConfiguration<EstadoCompra>
    {
        #region Métodos

        /// <summary>Método para realizar la configuración de la entidad según la BD.</summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<EstadoCompra> builder)
        {
            builder.ToTable("EstadoCompra", "Vuelo");

            builder.HasComment("Almacena la información de los estados que puede tener una compra dentro del sistema, como Pendiente, Pagada o Cancelada.");

            builder.HasKey(e => e.EstadoCompraId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.EstadoCompraId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del estado de compra.");

            builder.Property(e => e.EstadoCompraNombre)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre del estado de compra (por ejemplo, 'Pendiente', 'Pagada', 'Cancelada').");

            builder.Property(e => e.EstadoCompraDescripcion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("Descripción detallada del estado de compra.");

            builder.Property(e => e.EstadoCompraEstado)
                .IsRequired()
                .HasComment("Indica si el estado de compra está activo o inactivo. Valor 1 = Activo, 0 = Inactivo.");
        }

        #endregion
    }
}
