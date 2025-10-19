namespace Domain.General.DTOs.Avion
{
    public class AvionDto
    {
        /// <summary>Identificador único del avión.</summary>
        public int? AvionId { get; set; }

        /// <summary>Nombre del avión.</summary>
        public string? AvionNombre { get; set; }

        /// <summary>Identificador de la ciudad en donde se encuentra el avión.</summary>
        public int? CiudadId { get; set; }

        /// <summary>Nombre de la ciudad en donde se encuentra el avión.</summary>
        public string? CiudadNombre { get; set; }

        /// <summary>Nomenclatura de la ciudad en donde se encuentra el avión.</summary>
        public string? CiudadNomenclatura { get; set; }

        /// <summary>Nombre y nomenclatura de la ciudad en donde se encuentra el avión.</summary>
        public string? CiudadNombreNomenclatura { get; set; }

        /// <summary>Estado del avión. True si está activo, False si está inactivo.</summary>
        public bool? AvionEstado { get; set; }
    }
}
