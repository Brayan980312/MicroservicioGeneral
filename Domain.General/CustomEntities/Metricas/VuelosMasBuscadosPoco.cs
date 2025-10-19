namespace Domain.General.CustomEntities.Metricas
{
    public class VuelosMasBuscadosPoco
    {
        /// <summary>Nombre de la ciudad de origen.</summary>
        public string? CiudadOrigenNombre { get; set; }

        /// <summary>Nomenclatura de la ciudad de origen.</summary>
        public string? CiudadOrigenNomenclatura { get; set; }

        /// <summary>Nombre y nomenclatura de la ciudad de origen.</summary>
        public string? CiudadOrigenNombreNomenclatura { get; set; }

        /// <summary>Nombre de la ciudad de destino.</summary>
        public string? CiudadDestinoNombre { get; set; }

        /// <summary>Nomenclatura de la ciudad de destino.</summary>
        public string? CiudadDestinoNomenclatura { get; set; }

        /// <summary>Nombre y nomenclatura de la ciudad de destino.</summary>
        public string? CiudadDestinoNombreNomenclatura { get; set; }

        /// <summary>Indica la cantidad de veces que se está consultado un vuelo.</summary>
        public int? VuelosMasBuscadosCantidadActual { get; set; }
    }
}
