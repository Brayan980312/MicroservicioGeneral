namespace Infrastructure.General.External
{
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using Domain.General.Interfaces.External;
    using Utilitarios.Entities;
    public class ExternalService : IExternalService
    {
        private readonly HttpClient _httpClient;

        public ExternalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResultadoRespuestaObtenerDatos> ObtenerDatosAsync(string urlPeticion, List<KeyValuePair<string, string>> filtros)
        {
            ResultadoRespuestaObtenerDatos resultado = new ResultadoRespuestaObtenerDatos();

            string filtrosPeticion = string.Join("&", filtros
                .Where(f => !string.IsNullOrWhiteSpace(f.Value))
                .Select(f => $"{Uri.EscapeDataString(f.Key)}={Uri.EscapeDataString(f.Value)}"));

            string peticionURL = string.IsNullOrWhiteSpace(filtrosPeticion)
                ? urlPeticion
                : $"{urlPeticion}?{filtrosPeticion}";

            HttpResponseMessage respuesta = await _httpClient.GetAsync(peticionURL);
            string datos = await respuesta.Content.ReadAsStringAsync();
            int codigoRespuesta = (int)respuesta.StatusCode;

            if (codigoRespuesta == (int)Domain.General.Enums.Enum.RespuestaAPI.Ok)
            {
                resultado.Codigo = codigoRespuesta;
                resultado.Datos = datos;
            }
            else if (codigoRespuesta == (int)Domain.General.Enums.Enum.RespuestaAPI.Validacion)
            {
                var errorObj = JsonConvert.DeserializeObject<JObject>(datos);
                throw new ValidationException(errorObj?["detail"]?.ToString() ?? "Error de validación desconocido", new(datos));
            }
            else
            {
                string[] partes = urlPeticion.Split('/');
                string microServicio = partes.Length > 1 ? partes[^2] : "desconocido";
                throw new ValidationException($"Error en comunicación con el microservicio {microServicio}.", new(datos));
            }

            return resultado;
        }
    }
}
