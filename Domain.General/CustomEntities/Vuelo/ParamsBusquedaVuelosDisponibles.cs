namespace Domain.General.CustomEntities.Vuelo
{
    public class ParamsBusquedaVuelosDisponibles
    {
        /// <summary>Identificador de la ciudad de origen.</summary>
        public int CiudadOrigenId { get; set; }

        /// <summary>Identificador de la ciudad de destino.</summary>
        public int CiudadDestinoId { get; set; }

        /// <summary>Fecha de salida del vuelo.</summary>
        public DateTime VueloFechaSalida { get; set; }

        /// <summary>Identificador de la ciudad de destino.</summary>
        public int CantidadPasajeros { get; set; }
    }
}
