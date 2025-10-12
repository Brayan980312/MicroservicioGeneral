namespace Domain.General.Interfaces.External
{
    using Utilitarios.Entities;
    public interface IExternalService
    {
        /// <summary>Realiza peticiones a otro microservicio.</summary>
        /// <param name="urlPeticion">Endpoint que se va a consumir.</param>
        /// <param name="filtros">Filtros que se deben enviar con su propiedad y su valor.</param>
        /// <returns>String JSON con la información consultada.</returns>
        Task<ResultadoRespuestaObtenerDatos> ObtenerDatosAsync(string urlPeticion, List<KeyValuePair<string, string>> filtros);
    }
}
