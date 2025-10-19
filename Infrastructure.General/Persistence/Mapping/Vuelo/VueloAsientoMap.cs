namespace Infrastructure.General.Persistence.Mapping.Vuelo
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Configuración de la entidad VueloAsiento para su mapeo con la base de datos.
    /// </summary>
    public class VueloAsientoMap : IEntityTypeConfiguration<VueloAsiento>
    {
        #region Métodos

        /// <summary>Método para realizar la configuración de la entidad según la BD.</summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<VueloAsiento> builder)
        {
            builder.ToTable("VueloAsiento", "Vuelo",t => t.HasComment("Controla la disponibilidad y concurrencia de los asientos por vuelo."));

            builder.HasKey(e => e.VueloAsientoId);

            builder.Property(e => e.VueloAsientoId)
                .ValueGeneratedOnAdd();

            builder.Ignore(e => e.Id);

            builder.Property(e => e.VueloId)
                .IsRequired();

            builder.Property(e => e.AsientoAvionId)
                .IsRequired();

            builder.Property(e => e.VueloAsientoReservado)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.VueloAsientoComprado)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.VueloAsientoBloqueadoHasta)
                .HasColumnType("datetime");

            builder.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            builder.HasOne(e => e.AsientoAvion)
                .WithMany()
                .HasForeignKey(e => e.AsientoAvionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Vuelo_Avion_AvionId");
        }

        #endregion
    }
}
