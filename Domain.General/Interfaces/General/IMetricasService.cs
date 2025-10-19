namespace Domain.General.Interfaces.General
{
    using Domain.General.CustomEntities.Metricas;
    using Domain.General.Entities;
    using Utilitarios.Contracts.Crud;

    /// <summary>Define el contrato del servicio de metricas.
    /// <para>Este servicio permite operaciones de creacion y lectura de la entidad VuelosMasBuscados y VuelosMasComprados, utilizando los contratos genéricos <see cref="ICreateService{TParam, TEntity}"/> y <see cref="IReadWithParamsService{TParam, TEntity}"/>.</para>
    /// </summary>
    public interface IMetricasService : ICreateService<ParamsCrearActualizarVuelosMasBuscados, IEnumerable<VuelosMasBuscados>>,
                                                                        IReadWithParamsService<ParamsConsultarVuelosMasBuscados, IEnumerable<VuelosMasBuscados>>
    {
        /* Aquí se debe dejar el contrato personalizado */
        /// <summary>Contrato personalizado para la consulta de vuelos más buscados.</summary>
        /// <returns>Lista VuelosMasBuscadosPoco con la información de los vuelos más buscados.</returns>
        Task<IEnumerable<VuelosMasBuscadosPoco>> ConsultarVuelosMasBuscados();
    }
}
