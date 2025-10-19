namespace Infrastructure.General.Repository.Custom
{
    using Domain.General.CustomEntities.Metricas;
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.Entities;
    using Domain.General.Interfaces.Repository;
    using Infrastructure.General.Persistence.Context;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using Utilitarios.Data;
    public class DLMetricas : CrudSqlRepositorio<VuelosMasBuscados>, IDLMetricas
    {
        #region Variables

        /// <summary>Implementacion contexto.</summary>
        private readonly GeneralContext _GeneralContext;
        #endregion

        #region Constructor

        /// <summary>Inicializa la clase DLMetricas.</summary>
        /// <param name="GeneralContext">Contexto bd.</param>
        public DLMetricas(GeneralContext GeneralContext) : base(GeneralContext)
        {
            this._GeneralContext = GeneralContext;
        }

        #endregion

        /// <summary>Consulta personalizada que obtiene los vuelos más buscados en el sistema.</summary>
        /// <returns>Lista VuelosMasBuscadosPoco que cumplen con los filtros de búsqueda.</returns>
        public async Task<IEnumerable<VuelosMasBuscadosPoco>> ConsultarVuelosMasBuscados(int? topParametrizado)
        {
            var vuelosMasBuscados = await _GeneralContext.VuelosMasBuscados
                 .Include(ciudadOrigen => ciudadOrigen.CiudadOrigen)
                 .Include(ciudadDestino => ciudadDestino.CiudadDestino)
                 .OrderByDescending(v => v.VuelosMasBuscadosCantidadActual)
                 .Take((int)topParametrizado)
                 .Select(vuelosMasBuscados => new VuelosMasBuscadosPoco
                 {
                     CiudadOrigenNombre = vuelosMasBuscados.CiudadOrigen.CiudadNombre,
                     CiudadOrigenNomenclatura = vuelosMasBuscados.CiudadOrigen.CiudadNomenclatura,
                     CiudadOrigenNombreNomenclatura = vuelosMasBuscados.CiudadOrigen.CiudadNombre + " (" + vuelosMasBuscados.CiudadOrigen.CiudadNomenclatura + ")",
                     CiudadDestinoNombre = vuelosMasBuscados.CiudadDestino.CiudadNombre,
                     CiudadDestinoNomenclatura = vuelosMasBuscados.CiudadDestino.CiudadNomenclatura,
                     CiudadDestinoNombreNomenclatura = vuelosMasBuscados.CiudadDestino.CiudadNombre + " (" + vuelosMasBuscados.CiudadDestino.CiudadNomenclatura + ")",
                     VuelosMasBuscadosCantidadActual = vuelosMasBuscados.VuelosMasBuscadosCantidadActual
                 }
                 )
                 .AsNoTracking()
                 .ToListAsync();
            return vuelosMasBuscados;
        }
    }
}
