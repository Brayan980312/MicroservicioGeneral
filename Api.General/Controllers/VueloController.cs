namespace Api.General.Controllers
{
    using AutoMapper;
    using Domain.General.CustomEntities.Creditos;
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.DTOs.Creditos;
    using Domain.General.DTOs.Vuelo;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]

    public class VueloController : ControllerBase
    {
        #region Variables

        /// <summary>Inyección de Dependecias de servicios.</summary>
        private readonly IServiceUnitOfWork _iServiceUnitOfWork;

        /// <summary>Inyeccion de convertidor o resolutor de modelos.</summary>
        private readonly IMapper _iMapper;

        /// <summary>
        /// Obtiene una instancia del servicio de vuelos a través de la unidad de trabajo de servicios.
        /// </summary>
        private IVueloService VueloService => _iServiceUnitOfWork.GetService<IVueloService>();

        #endregion Variables

        #region Constructor

        /// <summary>Inicializa una nueva instancia de la clase VueloController.</summary>
        /// <param name="iMapper">Inyección de convertidor o resolutor de modelos.</param>
        /// <param name="iServiceUnitOfWork">Inyección de dependecias de Servicios.</param>
        public VueloController(IMapper iMapper, IServiceUnitOfWork iServiceUnitOfWork)
        {
            _iMapper = iMapper;
            _iServiceUnitOfWork = iServiceUnitOfWork;
        }

        #endregion

        #region EndPoints
        /// <summary>Endpoint para crear o actualizar la entidad.</summary>
        /// <param name="parametrosCrearActualizarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("CrearActualizarVuelo")]
        public async Task<ActionResult<VueloDto>> CrearActualizarVuelo(ParamsCrearActualizarVuelo parametrosCrearActualizarEntidad)
        {
            var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");

            Vuelo Entidad = await VueloService.CreateAsync(parametrosCrearActualizarEntidad, Convert.ToInt32(usuarioIdClaim.Value));
            return Ok(_iMapper.Map<VueloDto>(Entidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [HttpGet("ConsultarVuelo")]
        public async Task<ActionResult<List<VueloDto>>> ConsultarVuelo([FromQuery] ParamsConsultarVuelo parametrosConsultarEntidad)
        {
            IEnumerable<Vuelo> listadoEntidad = await VueloService.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<VueloDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet("ConsultarVueloHistorico")]
        public async Task<ActionResult<List<VueloHistoricoDto>>> ConsultarVueloHistorico([FromQuery] ParamsConsultarVueloHistorico parametrosConsultarEntidad)
        {
            IEnumerable<VueloHistorico> listadoEntidad = await VueloService.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<VueloHistoricoDto>(listadoEntidad));
        }

        /// <summary>Endpoint para crear o actualizar la entidad.</summary>
        /// <param name="parametrosCrearActualizarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [Route("CrearActualizarCompra")]
        public async Task<ActionResult<CompraDto>> CrearActualizarCompra(ParamsCrearActualizarCompra parametrosCrearActualizarEntidad)
        {
            var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");

            Compra Entidad = await VueloService.CreateAsync(parametrosCrearActualizarEntidad, Convert.ToInt32(usuarioIdClaim.Value));
            return Ok(_iMapper.Map<CompraDto>(Entidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Cliente")]
        [HttpGet("ConsultarCompra")]
        public async Task<ActionResult<List<CompraDto>>> ConsultarCompra([FromQuery] ParamsConsultarCompra parametrosConsultarEntidad)
        {
            var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");

            IEnumerable<Compra> listadoEntidad = await VueloService.GetWithParamsAsync(parametrosConsultarEntidad, Convert.ToInt32(usuarioIdClaim.Value));
            return Ok(_iMapper.Map<CompraDto>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [HttpGet("ConsultarCompraHistorico")]
        public async Task<ActionResult<List<CompraHistoricoDto>>> ConsultarCompraHistorico([FromQuery] ParamsConsultarCompraHistorico parametrosConsultarEntidad)
        {
            IEnumerable<CompraHistorico> listadoEntidad = await VueloService.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<CompraHistoricoDto>(listadoEntidad));
        }

        /// <summary>Endpoint para crear o actualizar la entidad.</summary>
        /// <param name="parametrosCrearActualizarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [Route("CrearActualizarCompraDetalle")]
        public async Task<ActionResult<CompraDetalleDto>> CrearActualizarCompraDetalle(List<ParamsCrearActualizarCompraDetalle> parametrosCrearActualizarEntidad)
        {
            CompraDetalle Entidad = await VueloService.CreateAsync(parametrosCrearActualizarEntidad);
            return Ok(_iMapper.Map<CompraDetalleDto>(Entidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Cliente")]
        [HttpGet("ConsultarCompraDetalle")]
        public async Task<ActionResult<List<CompraDetalleDto>>> ConsultarCompraDetalle([FromQuery] ParamsConsultarCompraDetalle parametrosConsultarEntidad)
        {
            IEnumerable<CompraDetalle> listadoEntidad = await VueloService.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<CompraDetalleDto>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Cliente")]
        [HttpGet("ConsultarCompraDetalleHistorico")]
        public async Task<ActionResult<List<CompraDetalleHistoricoDto>>> ConsultarCompraDetalleHistorico([FromQuery] ParamsConsultarCompraDetalleHistorico parametrosConsultarEntidad)
        {
            IEnumerable<CompraDetalleHistorico> listadoEntidad = await VueloService.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<CompraDetalleHistoricoDto>(listadoEntidad));
        }
        #endregion

    }
}