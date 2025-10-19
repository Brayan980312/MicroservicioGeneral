namespace Domain.General.Interfaces.Repository
{
    using Domain.General.CustomEntities.Metricas;
    using Domain.General.Entities;
    using Utilitarios.Contracts;
    public interface IDLMetricas : ICrudSqlRepositorio<VuelosMasBuscados>
    {
        /// <summary>Consulta personalizada que obtiene los vuelos más buscados en el sistema.</summary>
        /// <param name="topParametrizado">Indica sobre cual se debe realizar el Top de los vuelos más buscados.</param>
        /// <returns>Lista VuelosMasBuscadosPoco que cumplen con los filtros de búsqueda.</returns>
        Task<IEnumerable<VuelosMasBuscadosPoco>> ConsultarVuelosMasBuscados(int? topParametrizado);
    }
}
