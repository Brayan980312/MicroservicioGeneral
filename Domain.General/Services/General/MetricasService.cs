namespace Domain.General.Services.General
{
    using AutoMapper;
    using Domain.General.CustomEntities.Avion;
    using Domain.General.CustomEntities.Metricas;
    using Domain.General.CustomEntities.Params;
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using System.Linq.Expressions;
    using System.Transactions;
    using Utilitarios.Extensions;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class MetricasService : IMetricasService
    {
        #region Variables

        /// <summary>Instancia de la unidad de trabajo - UnitOfWork.</summary>
        private readonly IUnitOfWork _iUnitOfWork;

        /// <summary>Instancia del Mapeador - Imapper.</summary>
        private readonly IMapper _iMapper;

        /// <summary>Inyeccion del servicio IConfiguracionService.</summary>
        private readonly IConfiguracionService _iConfiguracionService;

        #endregion

        #region Constructor

        ///<summary>Inicializa una nueva instancia de la clase AvionService.</summary>
        /// <param name="iUnitOfWork">Inyección de dependencias de la unidad de trabajo - UnitOfWork.</param>
        public MetricasService(IUnitOfWork iUnitOfWork, IMapper iMapper, IConfiguracionService iConfiguracionService)
        {
            _iUnitOfWork = iUnitOfWork;
            _iMapper = iMapper;
            _iConfiguracionService = iConfiguracionService;
        }

        #endregion

        #region Métodos       

        /// <inheritdoc />
        public async Task<IEnumerable<VuelosMasBuscados>> CreateAsync(ParamsCrearActualizarVuelosMasBuscados paramsCreateUpdate, int? userId = null)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {

                // Valida si la metrica de ciudad origen y ciudad destino ya existe
                IEnumerable<VuelosMasBuscados> vuelosMasBuscados = new List<VuelosMasBuscados>();
                ParamsConsultarVuelosMasBuscados paramsConsultarVuelosMasBuscados = new ParamsConsultarVuelosMasBuscados();
                paramsConsultarVuelosMasBuscados.CiudadOrigenId = paramsCreateUpdate.CiudadOrigenId;
                paramsConsultarVuelosMasBuscados.CiudadDestinoId = paramsCreateUpdate.CiudadDestinoId;
                vuelosMasBuscados = await GetWithParamsAsync(paramsConsultarVuelosMasBuscados);
                VuelosMasBuscados agregarMetrica = new VuelosMasBuscados();

                if (vuelosMasBuscados.Count() > 0)
                {
                    agregarMetrica = vuelosMasBuscados.FirstOrDefault();
                    agregarMetrica.VuelosMasBuscadosCantidadActual += 1;

                    await _iUnitOfWork.Repository<VuelosMasBuscados>().ActualizarAsync(agregarMetrica);
                }
                else
                {
                    agregarMetrica.VuelosMasBuscadosId = null;
                    agregarMetrica.CiudadOrigenId = paramsCreateUpdate.CiudadOrigenId;
                    agregarMetrica.CiudadDestinoId = paramsCreateUpdate.CiudadDestinoId;
                    agregarMetrica.VuelosMasBuscadosCantidadActual = 1;
                    await _iUnitOfWork.Repository<VuelosMasBuscados>().AdicionarAsync(agregarMetrica);
                }

                await _iUnitOfWork.SaveChangesAsync();

                scope.Complete();
                return vuelosMasBuscados;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<VuelosMasBuscados>> GetWithParamsAsync(ParamsConsultarVuelosMasBuscados paramsSearch, int? userId = null)
        {
            VuelosMasBuscados entidad = _iMapper.Map<VuelosMasBuscados>(paramsSearch);
            Expression<Func<VuelosMasBuscados, bool>> filtro = entidad.ToFilterExpression<VuelosMasBuscados>();
            return await _iUnitOfWork.Repository<VuelosMasBuscados>().ConsultarListaAsync(filtro);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<VuelosMasBuscadosPoco>> ConsultarVuelosMasBuscados()
        {
            int? valorParametro = 10; // Toma los 10 primeros, o de acuerdo lo que este como parametro

            // Obtiene el parametro que define cuantos vuelos más buscados que se van a visualizar
            IEnumerable<Parametros> NumeroTopVuelosMasBuscados;
            ParamsConsultarParametros parametrosBusqueda = new ParamsConsultarParametros();
            parametrosBusqueda.ParametrosNombre = "NumeroTopVuelosMasBuscados";
            NumeroTopVuelosMasBuscados = await _iConfiguracionService.GetWithParamsAsync(parametrosBusqueda);

            if (NumeroTopVuelosMasBuscados.Count() > 0)
            {
                valorParametro = Convert.ToInt32(NumeroTopVuelosMasBuscados.FirstOrDefault()!.ParametrosValor);
            }
            return await _iUnitOfWork.DLMetricasPersonalizado.ConsultarVuelosMasBuscados(valorParametro);
        }
        #endregion
    }
}
