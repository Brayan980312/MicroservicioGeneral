namespace Domain.General.Entities
{
    using Utilitarios.Entities;
    /// <summary>Realiza la inicialización de las propiedades de la entidad Pais.</summary>
    public class Pais : EntidadBase
    {
        /// <summary>Identificador único del país.</summary>
        public int? PaisId { get; set; }

        /// <summary>Nombre completo del país.</summary>
        public string? PaisNombre { get; set; }

        /// <summary>Nomenclatura o código corto del país (por ejemplo, 'COL', 'USA').</summary>
        public string? PaisNomenclatura { get; set; }

        /// <summary>Indica si el país es considerado internacional. True si es internacional, False si no lo es.</summary>
        public bool? PaisInternacional { get; set; }

        /// <summary>Estado del país. True si está activo, False si está inactivo.</summary>
        public bool? PaisEstado { get; set; }
    }
}
