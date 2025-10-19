namespace Infrastructure.General.Repository.Custom
{
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.Entities;
    using Domain.General.Interfaces.Repository;
    using Infrastructure.General.Persistence.Context;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.EntityFrameworkCore;
    using Utilitarios.Data;
    public class DLCiudad : CrudSqlRepositorio<Ciudad>, IDLCiudad
    {
        #region Variables

        /// <summary>Implementacion contexto.</summary>
        private readonly GeneralContext _GeneralContext;
        #endregion

        #region Constructor

        /// <summary>Inicializa la clase DLCiudad.</summary>
        /// <param name="GeneralContext">Contexto bd.</param>
        public DLCiudad(GeneralContext GeneralContext) : base(GeneralContext)
        {
            this._GeneralContext = GeneralContext;
        }

        #endregion

        /// <summary>Consulta personalizada que obtiene las ciudades del sistema con su correspondiente pais.</summary>
        /// <param name="objSearch">Un objeto de tipo ParamsConsultarCiudad que contiene los criterios de búsqueda para consultar las ciudades del sistema.</param>
        /// <returns>Lista BusquedaCiudadPoco que cumplen con los filtros de búsqueda.</returns>
        public async Task<IEnumerable<BusquedaCiudadPoco>> ConsultarCiudades(ParamsConsultarCiudad objSearch)
        {
            var ciudades = await _GeneralContext.Ciudad
                 .Include(ciudad => ciudad.Pais)
                 .Where(ciudad => (ciudad.CiudadId == objSearch.CiudadId || objSearch.CiudadId == null || objSearch.CiudadId == 0) &&
                                                    (ciudad.CiudadNombre == objSearch.CiudadNombre || objSearch.CiudadNombre == null || objSearch.CiudadNombre == "") &&
                                                    (ciudad.CiudadNomenclatura == objSearch.CiudadNomenclatura || objSearch.CiudadNomenclatura == null || objSearch.CiudadNomenclatura == "") &&
                                                    (ciudad.PaisId == objSearch.PaisId || objSearch.PaisId == null || objSearch.PaisId == 0) &&
                                                    (ciudad.CiudadEstado == objSearch.CiudadEstado || objSearch.CiudadEstado == null))
                 .Select(ciudades => new BusquedaCiudadPoco
                 {
                     CiudadId = ciudades.CiudadId,
                     CiudadNombre = ciudades.CiudadNombre,
                     CiudadNomenclatura = ciudades.CiudadNomenclatura,
                     CiudadNombreNomenclatura = ciudades.CiudadNombre + " (" + ciudades.CiudadNomenclatura+ ")",
                     PaisId = ciudades.PaisId,
                     PaisNombre = ciudades.Pais.PaisNombre,
                     PaisNomenclatura = ciudades.Pais.PaisNomenclatura,
                     PaisNombreNomenclatura = ciudades.Pais.PaisNombre + " ("+ciudades.Pais.PaisNomenclatura+")",
                     CiudadEstado = ciudades.CiudadEstado,
                 }
                 )
                 .AsNoTracking()
                 .ToListAsync();
            return ciudades;
        }
    }
}
