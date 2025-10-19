namespace Infrastructure.General.Persistence.Context
{
    using Domain.General.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>DbContext para las entidades relacionadas con las entidades del proyecto y auditoría.
    /// Contiene la configuración y DbSet de la tabla de auditoría y aplica los mapeos correspondientes.
    /// </summary>
    public class GeneralContext : DbContext
    {
        #region Constructores

        /// <summary>Constructor por defecto. Necesario para algunas herramientas (EF tools, scaffolding).</summary>
        public GeneralContext()
        {
        }

        /// <summary>Constructor que recibe las opciones del DbContext (inyección de dependencias).</summary>
        /// <param name="options">Opciones de configuración del contexto.</param>
        public GeneralContext(DbContextOptions<GeneralContext> options)
            : base(options)
        {
        }

        #endregion

        #region Configuración del Context

        /// <summary>Configuración opcional del contexto. Si no está configurado desde fuera, aplica un comportamiento por defecto (NoTracking para lecturas).</summary>
        /// <param name="optionsBuilder">Constructor de opciones del contexto.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Sólo aplicamos la configuración por defecto si el contexto no fue configurado desde fuera.
            if (!optionsBuilder.IsConfigured)
            {
                // NoTracking por defecto para evitar overhead en consultas de solo lectura.
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        }

        /// <summary>Configura el modelo aplicando los mapeos (IEntityTypeConfiguration).</summary>
        /// <param name="modelBuilder">ModelBuilder para configurar el modelo.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Forzar collation a nivel EF si es necesario en tu entorno.
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            // Aplica cualquier otro mapping que exista en el ensamblado.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GeneralContext).Assembly);
        }

        #endregion

        #region EntidadesPersonalizadas
        /// <value>Declaración de DbSet Ciudad.</value>
        public virtual DbSet<Ciudad> Ciudad { get; set; }

        /// <value>Declaración de DbSet Avion.</value>
        public virtual DbSet<Avion> Avion { get; set; }

        /// <value>Declaración de DbSet Vuelo.</value>
        public virtual DbSet<Vuelo> Vuelo { get; set; }

        /// <value>Declaración de DbSet VueloAsiento.</value>
        public virtual DbSet<VueloAsiento> VueloAsiento { get; set; }

        /// <value>Declaración de DbSet VuelosMasBuscados.</value>
        public virtual DbSet<VuelosMasBuscados> VuelosMasBuscados { get; set; }

        /// <value>Declaración de DbSet VueloHistorico.</value>
        public virtual DbSet<VueloHistorico> VueloHistorico { get; set; }

        #endregion
    }
}
