namespace Infrastructure.General.Repository.Custom
{
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.CustomEntities.Avion;
    using Domain.General.Entities;
    using Domain.General.Interfaces.Repository;
    using Infrastructure.General.Persistence.Context;
    using Microsoft.EntityFrameworkCore;
    using Utilitarios.Data;
    public class DLAvion : CrudSqlRepositorio<Avion>, IDLAvion
    {
        #region Variables

        /// <summary>Implementacion contexto.</summary>
        private readonly GeneralContext _GeneralContext;
        #endregion

        #region Constructor

        /// <summary>Inicializa la clase DLAvion.</summary>
        /// <param name="GeneralContext">Contexto bd.</param>
        public DLAvion(GeneralContext GeneralContext) : base(GeneralContext)
        {
            this._GeneralContext = GeneralContext;
        }

        #endregion

        /// <summary>Consulta personalizada que obtiene los aviones del sistema con su correspondiente avion.</summary>
        /// <param name="objSearch">Un objeto de tipo ParamsConsultarAvion que contiene los criterios de búsqueda para consultar los aviones del sistema.</param>
        /// <returns>Lista BusquedaAvionPoco que cumplen con los filtros de búsqueda.</returns>
        public async Task<IEnumerable<BusquedaAvionPoco>> ConsultarAviones(ParamsConsultarAvion objSearch)
        {
            var aviones = await _GeneralContext.Avion
                 .Include(avion => avion.Ciudad)
                 .Where(avion => (avion.AvionId == objSearch.AvionId || objSearch.AvionId == null || objSearch.AvionId == 0) &&
                                                  (avion.AvionNombre == objSearch.AvionNombre || objSearch.AvionNombre == null || objSearch.AvionNombre == "") &&
                                                  (avion.CiudadId == objSearch.CiudadId || objSearch.CiudadId == null || objSearch.CiudadId == 0) &&
                                                  (avion.AvionEstado == objSearch.AvionEstado || objSearch.AvionEstado == null))
                 .Select(aviones => new BusquedaAvionPoco
                 {
                     AvionId = aviones.AvionId,
                     AvionNombre = aviones.AvionNombre,
                     CiudadId = aviones.CiudadId,
                     CiudadNombre = aviones.Ciudad.CiudadNombre,
                     CiudadNomenclatura = aviones.Ciudad.CiudadNomenclatura,
                     CiudadNombreNomenclatura = aviones.Ciudad.CiudadNombre + " (" + aviones.Ciudad.CiudadNomenclatura + ")",
                     AvionEstado = aviones.AvionEstado,
                 }
                 )
                 .AsNoTracking()
                 .ToListAsync();
            return aviones;
        }
    }
}
