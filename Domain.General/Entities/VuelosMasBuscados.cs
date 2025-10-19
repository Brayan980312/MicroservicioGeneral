namespace Domain.General.Entities
{
    using Utilitarios.Entities;

    /// <summary>Representa las métricas de los vuelos más buscados.</summary>
    public class VuelosMasBuscados : EntidadBase
    {
        /// <summary>Identificador único del registro de vuelos más buscados.</summary>
        public int? VuelosMasBuscadosId { get; set; }

        /// <summary>Identificador de la ciudad de origen.</summary>
        public int? CiudadOrigenId { get; set; }

        /// <summary>Identificador de la ciudad de destino.</summary>
        public int? CiudadDestinoId { get; set; }

        /// <summary>Cantidad total de veces que se ha buscado este vuelo.</summary>
        public int? VuelosMasBuscadosCantidadActual { get; set; }

        /// <summary>Entidad de navegación hacia la ciudad de origen.</summary>
        public Ciudad? CiudadOrigen { get; set; }

        /// <summary>Entidad de navegación hacia la ciudad de destino.</summary>
        public Ciudad? CiudadDestino { get; set; }
    }
}
