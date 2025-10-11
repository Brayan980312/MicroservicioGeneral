namespace Domain.General.Interfaces.General
{
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.Entities;
    using Utilitarios.Contracts.Crud;

    /// <summary>Define el contrato del servicio de administracion.
    /// <para>Este servicio permite operaciones de actualización y lectura de la entidad pais, ciudad y metodo pago, utilizando los contratos genéricos <see cref="IUpdateService{TParam, TEntity}"/> y <see cref="IReadWithParamsService{TParam, TEntity}"/>.</para>
    /// </summary>
    public interface IAdministracionService : IUpdateService<ParamsCrearActualizarPais, IEnumerable<Pais>>, 
                                                                                    IReadWithParamsService<ParamsConsultarPais, Pais>, 
                                                                                    IUpdateService<ParamsCrearActualizarCiudad, IEnumerable<Ciudad>>, 
                                                                                    IReadWithParamsService<ParamsConsultarCiudad, Ciudad>,
                                                                                    IUpdateService<ParamsCrearActualizarMetodoPago, IEnumerable<MetodoPago>>,
                                                                                    IReadWithParamsService<ParamsConsultarMetodoPago, MetodoPago>
    {
        /* Aquí se debe dejar el contrato personalizado */
    }
}
