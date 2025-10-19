namespace Domain.General.Interfaces.Repository
{
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.Entities;
    using Utilitarios.Contracts;
    public interface IDLCiudad : ICrudSqlRepositorio<Ciudad> 
    {
        /// <summary>Consulta personalizada que obtiene las ciudades del sistema con su correspondiente pais.</summary>
        /// <param name="objSearch">Un objeto de tipo ParamsConsultarCiudad que contiene los criterios de búsqueda para consultar las ciudades del sistema.</param>
        /// <returns>Lista BusquedaCiudadPoco que cumplen con los filtros de búsqueda.</returns>
        Task<IEnumerable<BusquedaCiudadPoco>> ConsultarCiudades(ParamsConsultarCiudad objSearch);
    }
}
