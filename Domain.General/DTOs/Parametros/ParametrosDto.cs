namespace Domain.General.DTOs.Parametros
{
    public class ParametrosDto
    {
        /// <summary>Identificador único del parámetro.</summary>
        public int? ParametrosId { get; set; }

        /// <summary>Nombre descriptivo del parámetro.</summary>
        public string? ParametrosNombre { get; set; }

        /// <summary>Valor asignado al parámetro.</summary>
        public string? ParametrosValor { get; set; }

        /// <summary>Descripcion del parámetro.</summary>
        public string? ParametrosDescripcion { get; set; }

        /// <summary>Estado del parámetro. True si está activo, False si está inactivo.</summary>
        public bool? ParametrosEstado { get; set; }
    }
}
