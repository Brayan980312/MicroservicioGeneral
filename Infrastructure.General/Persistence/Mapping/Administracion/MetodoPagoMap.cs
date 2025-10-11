namespace Infrastructure.General.Persistence.Mapping.Administracion
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MetodoPagoMap : IEntityTypeConfiguration<MetodoPago>
    {
        #region Métodos
        /// <summary>
        /// Configura la entidad <see cref="MetodoPago"/> según su estructura en la base de datos.
        /// </summary>
        /// <param name="builder">Constructor de la entidad a configurar.</param>
        public void Configure(EntityTypeBuilder<MetodoPago> builder)
        {
            builder.ToTable("MetodoPago", "Administracion");

            builder.HasComment("Almacena la información de los métodos de pago disponibles en el sistema, incluyendo su descripción y estado activo/inactivo.");

            builder.HasKey(e => e.MetodoPagoId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.MetodoPagoId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del método de pago.");

            builder.Property(e => e.MetodoPagoNombre)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre del método de pago (por ejemplo, 'Efectivo', 'Transferencia', 'Tarjeta').");

            builder.Property(e => e.MetodoPagoDescripcion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("Descripción adicional del método de pago.");

            builder.Property(e => e.MetodoPagoEstado)
                .IsRequired()
                .HasComment("Estado del método de pago. Valor 1 = Activo, 0 = Inactivo.");
        }
        #endregion
    }
}
