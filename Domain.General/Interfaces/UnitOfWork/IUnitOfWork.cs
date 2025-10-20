namespace Domain.General.Interfaces.UnitOfWork
{
    using Domain.General.Interfaces.Repository;
    using System;
    using System.Threading.Tasks;
    using Utilitarios.Contracts;
    using Utilitarios.Entities;

    /// <summary>Contrato para la Unidad de Trabajo (Unit of Work).
    /// Coordina los repositorios (genéricos y especializados) y gestiona la persistencia de cambios en la base de datos.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region Repositorios

        /// <summary>Obtiene una instancia del repositorio genérico para la entidad especificada.</summary>
        /// <typeparam name="T">Tipo de entidad que hereda de <see cref="EntidadBase"/>.</typeparam>
        /// <returns>Una instancia de <see cref="ICrudSqlRepositorio{T}"/> asociada a la entidad <typeparamref name="T"/>.</returns>
        ICrudSqlRepositorio<T> Repository<T>() where T : EntidadBase;

        /// <summary>Instancia del repositorio de DLCiudad</summary>
        IDLCiudad DLCiudadPersonalizado { get; }

        /// <summary>Instancia del repositorio de DLAvion</summary>
        IDLAvion DLAvionPersonalizado { get; }

        /// <summary>Instancia del repositorio de DLVuelo</summary>
        IDLVuelo DLVueloPersonalizado { get; }

        /// <summary>Instancia del repositorio de DLMetricas</summary>
        IDLMetricas DLMetricasPersonalizado { get; }

        /// <summary>Instancia del repositorio de DLCompras</summary>
        IDLCompras DLComprasPersonalizado { get; }

        #endregion

        #region Persistencia

        /// <summary>Guarda de manera sincrónica todos los cambios efectuados en el contexto de datos.</summary>
        void SaveChanges();

        /// <summary>Guarda de manera asincrónica todos los cambios efectuados en el contexto de datos.</summary>
        /// <returns>Una tarea asincrónica que representa la operación de guardado.</returns>
        Task SaveChangesAsync();

        #endregion
    }
}
