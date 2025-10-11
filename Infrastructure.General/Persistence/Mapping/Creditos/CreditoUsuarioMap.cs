namespace Infrastructure.General.Persistence.Mapping.Creditos
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CreditoUsuarioMap : IEntityTypeConfiguration<CreditoUsuario>
    {
        #region Métodos

        /// <summary>
        /// Método para realizar la configuración de la entidad según la BD.
        /// </summary>
        /// <param name="builder">Entidad a Configurar.</param>
        public void Configure(EntityTypeBuilder<CreditoUsuario> builder)
        {
            builder.ToTable("CreditoUsuario", "Creditos");

            builder.HasComment("Almacena los créditos asignados a cada usuario dentro del sistema.");

            builder.HasKey(e => e.CreditoUsuarioId);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.CreditoUsuarioId)
                .ValueGeneratedOnAdd()
                .HasComment("Identificador único del registro de créditos del usuario.");

            builder.Property(e => e.UsuarioId)
                .IsRequired()
                .HasComment("Identificador del usuario al que pertenecen los créditos.");

            builder.Property(e => e.CreditoUsuarioCreditos)
                .HasColumnType("decimal(18,2)")
                .IsRequired()
                .HasComment("Cantidad total de créditos disponibles para el usuario.");
        }

        #endregion
    }
}
