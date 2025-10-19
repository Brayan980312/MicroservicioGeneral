namespace Domain.General.Entities
{
    using Utilitarios.Entities;

    /// <summary>Representa la información de un avión.</summary>
    public class Avion : EntidadBase
    {
        /// <summary>Identificador único del avión.</summary>
        public int? AvionId { get; set; }

        /// <summary>Nombre del avión.</summary>
        public string? AvionNombre { get; set; }

        /// <summary>Identificador de la ciudad en donde se encuentra el avión.</summary>
        public int? CiudadId { get; set; }

        /// <summary>Estado del avión. True si está activo, False si está inactivo.</summary>
        public bool? AvionEstado { get; set; }

        /// <summary>Entidad de navegación hacia la ciudad en donde se encuentra el avión.</summary>
        public Ciudad? Ciudad { get; set; }
    }
}
