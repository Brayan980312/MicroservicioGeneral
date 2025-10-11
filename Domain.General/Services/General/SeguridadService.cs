namespace Domain.General.Services.General
{
    using Domain.General.Entities;
    using Domain.General.Interfaces.External;
    using Domain.General.Interfaces.General;
    using Newtonsoft.Json;
    using Utilitarios.Constants;
    using Utilitarios.Entities;

    public class SeguridadService : ISeguridadService
    {
        /// <summary>Inyeccion del servicio IExternalService.</summary>
        private readonly IExternalService _iExternalService;

        ///<summary>Inicializa una nueva instancia de la clase CreditosService.</summary>
        /// <param name="iExternalService">Implementación de servicio externo.</param>
        public SeguridadService(IExternalService iExternalService)
        {
            _iExternalService = iExternalService;
        }

        /// <summary>Realiza petición al microservicio de seguridad para validar que existe el usuario.</summary>
        /// <param name="usuarioId">UsuarioId que se enviará a la petición externa para validar que el usuario existe.</param>
        /// <returns>Mensaje informando si existe o no el usuario.</returns>
        public async Task<string> ConsultarUsuarioExistente(int? usuarioId)
        {
            string mensaje = "";
            var filtros = new List<KeyValuePair<string, string>>
                {
                    new("usuarioId", usuarioId.ToString())
                };

            // Realiza la petición al microservicio de seguridad para validar si el usuario existe
            ResultadoRespuestaObtenerDatos resultado = await _iExternalService.ObtenerDatosAsync("Seguridad/ConsultaUsuario", filtros);
            if (resultado != null)
            {
                if (resultado.Codigo == (int)Domain.General.Enums.Enum.RespuestaAPI.Ok)
                {
                    Usuario usuario = JsonConvert.DeserializeObject<Usuario>(resultado.Datos);
                    if (usuario.UsuarioId == null || usuario.UsuarioId == 0)
                    {
                        mensaje = string.Format(DefaultMessages.DataNotFound, "El usuario");
                    }
                }
            }

            return mensaje;
        }
    }
}
