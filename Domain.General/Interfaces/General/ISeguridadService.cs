namespace Domain.General.Interfaces.General
{
    public interface ISeguridadService
    {
        /// <summary>Realiza la petición al microservicio de seguridad para validar que existe un usuario por su Id.</summary>
        /// <param name="usuarioId">UsuarioId que se enviará para consultar.</param>
        /// <returns>Objeto con el token JWT y la información del usuario.</returns>
        Task<string> ConsultarUsuarioExistente(int? usuarioId);
    }
}
