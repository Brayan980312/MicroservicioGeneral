namespace Domain.General.CustomEntities.Params
{
    /// <summary>Realiza la inicialización de las propiedades de la entidad ParametrosActualizarParametro.</summary>
    public class ParamsActualizarParametros
    {
        /// <summary>Identificador único del parámetro.</summary>
        public int? ParametrosId { get; set; }

        /// <value>Nombre descriptivo del parámetro.</value>
        public string? ParametrosNombre { get; set; }

        /// <summary>Valor asignado al parámetro.</summary>
        public string? ParametrosValor { get; set; }

        /// <value>Descripción del parámetro.</value>
        public string? ParametrosDescripcion { get; set; }

        /// <value>Estado del parámetro. True si está activo, False si está inactivo.</value>
        public bool? ParametrosEstado { get; set; }
    }
}
