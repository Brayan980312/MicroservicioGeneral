namespace Infrastructure.General.Persistence.Mapping.Vuelo
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CompraMap : IEntityTypeConfiguration<Compra>
    {
        #region Métodos

        /// <summary>
        /// Método para realizar la configuración de la entidad según la BD.
        /// </summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<Compra> builder)
        {
            builder.ToTable("Compra", "Vuelo");

            builder.HasComment("Registra las compras de vuelos realizadas por los usuarios.");

            builder.HasKey(e => e.CompraId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.CompraId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único de la compra.");

            builder.Property(e => e.UsuarioId)
                .IsRequired()
                .HasComment("Identificador del usuario que realiza la compra.");

            builder.Property(e => e.VueloId)
                .IsRequired()
                .HasComment("Identificador del vuelo adquirido.");

            builder.Property(e => e.EstadoCompraId)
                .IsRequired()
                .HasComment("Identificador del estado de la compra.");

            builder.Property(e => e.CompraFecha)
                .IsRequired()
                .HasComment("Fecha en que se realizó la compra.");

            builder.Property(e => e.MetodoPagoId)
                .IsRequired()
                .HasComment("Identificador del método de pago utilizado.");

            builder.Property(e => e.CompraTotal)
                .HasColumnType("decimal(18,2)")
                .IsRequired()
                .HasComment("Total de la compra.");
        }

        #endregion
    }
}
