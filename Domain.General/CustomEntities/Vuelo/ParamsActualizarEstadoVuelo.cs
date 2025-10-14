namespace Domain.General.CustomEntities.Vuelo
{
    public class ParamsActualizarEstadoVuelo
    {
        /// <summary>Identificador único del vuelo.</summary>
        public int? VueloId { get; set; }

        /// <summary>Identificador del estado del vuelo.</summary>
        public int? EstadoVueloId { get; set; }
    }
}
