namespace Domain.General.Entities
{
    using Utilitarios.Entities;

    /// <summary>Representa el estado actual de un vuelo dentro del sistema.</summary>
    public class EstadoVuelo : EntidadBase
    {
        /// <summary>Identificador único del estado de vuelo.</summary>
        public int? EstadoVueloId { get; set; }

        /// <summary>Nombre del estado de vuelo (por ejemplo, 'Programado', 'En vuelo', 'Cancelado').</summary>
        public string? EstadoVueloNombre { get; set; }

        /// <summary>Descripción detallada del estado de vuelo.</summary>
        public string? EstadoVueloDescripcion { get; set; }

        /// <summary>Indica si el estado de vuelo está activo o inactivo. True si está activo, False si no lo está.</summary>
        public bool? EstadoVueloEstado { get; set; }
    }
}