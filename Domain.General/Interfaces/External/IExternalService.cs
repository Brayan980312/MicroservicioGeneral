namespace Domain.General.Interfaces.External
{
    using Utilitarios.Entities;
    public interface IExternalService
    {
        /// <summary>Realiza peticiones a otro microservicio.</summary>
        /// <param name="urlPeticion">Ruta del microservicio que se va a consumir junto con su endpoint.</param>
        /// <param name="filtros">Filtros que se deben enviar con su valor y su resultado.</param>
        /// <returns>String JSON con la información consultada.</returns>
        Task<ResultadoRespuestaObtenerDatos> ObtenerDatosAsync(string urlPeticion, List<KeyValuePair<string, string>> filtros);
    }
}
