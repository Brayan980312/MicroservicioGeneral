namespace Infrastructure.General.Repository
{
    using Domain.General.Interfaces.Repository;
    using Domain.General.Interfaces.UnitOfWork;
    using Infrastructure.General.Persistence.Context;
    using Infrastructure.General.Repository.Custom;
    using Microsoft.EntityFrameworkCore;
    using Utilitarios.Contracts;
    using Utilitarios.Data;
    using Utilitarios.Entities;
    /// <summary>Implementación del patrón Unit of Work.
    /// Encapsula el contexto de base de datos y coordina los repositorios (genéricos y especializados).
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Variables
        private readonly GeneralContext _contexto;
        private readonly Dictionary<Type, object> _repositories = new();


        /// <summary>Instancia del repositorio - DLCiudad.</summary>
        private readonly IDLCiudad _iDLCiudadPersonalizado;

        /// <summary>Instancia del repositorio - DLAvion.</summary>
        private readonly IDLAvion _iDLAvionPersonalizado;

        /// <summary>Instancia del repositorio - DLVuelo.</summary>
        private readonly IDLVuelo _iDLVueloPersonalizado;

        /// <summary>Instancia del repositorio - DLMetricas.</summary>
        private readonly IDLMetricas _iDLMetricasPersonalizado;

        /// <summary>Instancia del repositorio - DLCompras.</summary>
        private readonly IDLCompras _iDLComprasPersonalizado;
        #endregion

        #region Constructor
        /// <summary>Inicializa una nueva instancia de la clase <see cref="UnitOfWork"/>.</summary>
        /// <param name="contexto">Contexto de base de datos de Entity Framework utilizado para persistir cambios.</param>
        public UnitOfWork(GeneralContext contexto)
        {
            _contexto = contexto;
        }

        #endregion

        #region Instancias
        /// <summary>Obtiene una instancia del repositorio genérico para la entidad especificada.
        /// Si no existe aún en el diccionario de repositorios, se crea una nueva instancia.
        /// </summary>
        /// <typeparam name="T">Tipo de entidad que hereda de <see cref="EntidadBase"/>.</typeparam>
        /// <returns>Una instancia de <see cref="ICrudSqlRepositorio{T}"/> asociada a la entidad <typeparamref name="T"/>.</returns>
        public ICrudSqlRepositorio<T> Repository<T>() where T : EntidadBase
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                var repoInstance = new CrudSqlRepositorio<T>(_contexto);
                _repositories.Add(type, repoInstance);
            }

            return (ICrudSqlRepositorio<T>)_repositories[type];
        }

        /// <summary>Inicialización y verificación de la instancia del repositorio - DLCiudadPersonalizado.</summary>
        public IDLCiudad DLCiudadPersonalizado => _iDLCiudadPersonalizado ?? new DLCiudad(_contexto);

        /// <summary>Inicialización y verificación de la instancia del repositorio - DLAvionPersonalizado.</summary>
        public IDLAvion DLAvionPersonalizado => _iDLAvionPersonalizado ?? new DLAvion(_contexto);

        /// <summary>Inicialización y verificación de la instancia del repositorio - DLVueloPersonalizado.</summary>
        public IDLVuelo DLVueloPersonalizado => _iDLVueloPersonalizado ?? new DLVuelo(_contexto);

        /// <summary>Inicialización y verificación de la instancia del repositorio - DLMetricas.</summary>
        public IDLMetricas DLMetricasPersonalizado => _iDLMetricasPersonalizado ?? new DLMetricas(_contexto);

        /// <summary>Inicialización y verificación de la instancia del repositorio - DLCompras.</summary>
        public IDLCompras DLComprasPersonalizado => _iDLComprasPersonalizado ?? new DLCompras(_contexto);
        #endregion

        #region Guardar Cambios

        /// <summary>Guardar cambios efectuados en la Conexión de BD.</summary>
        public void SaveChanges() => _contexto.SaveChanges();

        /// <summary>Guardar cambios efectuados en la Conexión de BD.</summary>
        public async Task SaveChangesAsync() => await _contexto.SaveChangesAsync();

        #endregion

        #region Liberar Conexión

        /// <summary>Libera la Conexión de BD.</summary>
        public void Dispose()
        {
            if (_contexto != null)
                _contexto.Dispose();
        }

        /// <summary>Libera la Conexión de BD.</summary>
        public async Task DisposeAsync()
        {
            if (_contexto != null)
                await _contexto.DisposeAsync();
        }

        #endregion
    }
}