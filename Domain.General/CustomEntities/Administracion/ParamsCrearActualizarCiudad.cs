namespace Domain.General.CustomEntities.Administracion
{
    public class ParamsCrearActualizarCiudad
    {
        /// <summary>
        /// Identificador único de la ciudad.
        /// </summary>
        public int? CiudadId { get; set; }

        /// <summary>
        /// Nombre completo de la ciudad.
        /// </summary>
        public string? CiudadNombre { get; set; }

        /// <summary>
        /// Nomenclatura o código corto de la ciudad (por ejemplo, 'BOG', 'MED').
        /// </summary>
        public string? CiudadNomenclatura { get; set; }

        /// <summary>
        /// Identificador del país al que pertenece la ciudad.
        /// </summary>
        public int? PaisId { get; set; }

        /// <summary>
        /// Estado de la ciudad. True si está activa, False si está inactiva.
        /// </summary>
        public bool? CiudadEstado { get; set; }
    }
}
