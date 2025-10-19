namespace Domain.General.CustomEntities.Metricas
{
    public class ParamsCrearActualizarVuelosMasBuscados
    {
        /// <summary>Identificador de la ciudad de origen.</summary>
        public int? CiudadOrigenId { get; set; }

        /// <summary>Identificador de la ciudad de destino.</summary>
        public int? CiudadDestinoId { get; set; }
    }
}
