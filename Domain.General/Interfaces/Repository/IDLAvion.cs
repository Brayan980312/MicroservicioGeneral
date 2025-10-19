namespace Domain.General.Interfaces.Repository
{
    using Domain.General.CustomEntities.Avion;
    using Domain.General.Entities;
    using Utilitarios.Contracts;

    public interface IDLAvion : ICrudSqlRepositorio<Avion> 
    {
        /// <summary>Consulta personalizada que obtiene los aviones del sistema con su correspondiente ciudad.</summary>
        /// <param name="objSearch">Un objeto de tipo ParamsConsultarAvion que contiene los criterios de búsqueda para consultar los aviones del sistema.</param>
        /// <returns>Lista BusquedaAvionPoco que cumplen con los filtros de búsqueda.</returns>
        Task<IEnumerable<BusquedaAvionPoco>> ConsultarAviones(ParamsConsultarAvion objSearch);
    }
}
