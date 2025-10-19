using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.General.CustomEntities.Administracion
{
    public class BusquedaCiudadPoco
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
        /// Nombre completo de la ciudad y la nomenclatura.
        /// </summary>
        public string? CiudadNombreNomenclatura { get; set; }

        /// <summary>
        /// Identificador del país al que pertenece la ciudad.
        /// </summary>
        public int? PaisId { get; set; }

        /// <summary>
        /// Nombre del pais asociado a la ciudad.
        /// </summary>
        public string? PaisNombre { get; set; }

        /// <summary>
        /// Nomenclatura del pais asociado a la ciudad.
        /// </summary>
        public string? PaisNomenclatura { get; set; }

        /// <summary>
        /// Nombre y nomenclatura del pais  asociado a la ciudad.
        /// </summary>
        public string? PaisNombreNomenclatura { get; set; }

        /// <summary>
        /// Estado de la ciudad. True si está activa, False si está inactiva.
        /// </summary>
        public bool? CiudadEstado { get; set; }
    }
}
