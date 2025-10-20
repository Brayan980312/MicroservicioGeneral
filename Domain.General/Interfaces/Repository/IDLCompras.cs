namespace Domain.General.Interfaces.Repository
{
    using Domain.General.CustomEntities.Compras;
    public interface IDLCompras
    {
        /// <summary>Consulta personalizada que obtiene las compras realizadas por el usuario que realiza la petición.</summary>
        /// <param name="userId">Usuario que realiza la petición.</param>
        /// <returns>Lista ComprasRealizadasUsuarioPoco que cumplen con los filtros de búsqueda.</returns>
        Task<IEnumerable<ComprasRealizadasUsuarioPoco>> BuscarComprasUsuarioAsycn(int userId);
    }
}
