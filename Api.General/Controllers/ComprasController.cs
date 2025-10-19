namespace Api.General.Controllers
{
    using AutoMapper;
    using Domain.General.CustomEntities.Compras;
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.DTOs.Compras;
    using Domain.General.DTOs.Vuelo;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using Domain.General.Services.General;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controlador que maneja los endpoints relacionados la compra de los vuelos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class ComprasController : ControllerBase
    {
        #region Variables

        /// <summary>Inyección de Dependecias de servicios.</summary>
        private readonly IServiceUnitOfWork _iServiceUnitOfWork;

        /// <summary>Inyeccion de convertidor o resolutor de modelos.</summary>
        private readonly IMapper _iMapper;

        /// <summary>
        /// Obtiene una instancia del servicio de compras a través de la unidad de trabajo de servicios.
        /// </summary>
        private IComprasService ComprasService => _iServiceUnitOfWork.GetService<IComprasService>();

        #endregion Variables

        #region Constructor

        /// <summary>Inicializa una nueva instancia de la clase ComprasController.</summary>
        /// <param name="iMapper">Inyección de convertidor o resolutor de modelos.</param>
        /// <param name="iServiceUnitOfWork">Inyección de dependecias de Servicios.</param>
        public ComprasController(IMapper iMapper, IServiceUnitOfWork iServiceUnitOfWork)
        {
            _iMapper = iMapper;
            _iServiceUnitOfWork = iServiceUnitOfWork;
        }

        #endregion

        #region EndPoints

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Cliente")]
        [HttpGet("ConsultarCompra")]
        public async Task<ActionResult<List<CompraDto>>> ConsultarCompra([FromQuery] ParamsConsultarCompra parametrosConsultarEntidad)
        {
            var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");

            IEnumerable<Compra> listadoEntidad = await ComprasService.GetWithParamsAsync(parametrosConsultarEntidad, Convert.ToInt32(usuarioIdClaim!.Value));
            return Ok(_iMapper.Map<CompraDto>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [HttpGet("ConsultarCompraHistorico")]
        public async Task<ActionResult<List<CompraHistoricoDto>>> ConsultarCompraHistorico([FromQuery] ParamsConsultarCompraHistorico parametrosConsultarEntidad)
        {
            IEnumerable<CompraHistorico> listadoEntidad = await ComprasService.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<CompraHistoricoDto>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Cliente")]
        [HttpGet("ConsultarCompraDetalle")]
        public async Task<ActionResult<List<CompraDetalleDto>>> ConsultarCompraDetalle([FromQuery] ParamsConsultarCompraDetalle parametrosConsultarEntidad)
        {
            IEnumerable<CompraDetalle> listadoEntidad = await ComprasService.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<CompraDetalleDto>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Objeto de Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Cliente")]
        [HttpGet("ConsultarCompraDetalleHistorico")]
        public async Task<ActionResult<List<CompraDetalleHistoricoDto>>> ConsultarCompraDetalleHistorico([FromQuery] ParamsConsultarCompraDetalleHistorico parametrosConsultarEntidad)
        {
            IEnumerable<CompraDetalleHistorico> listadoEntidad = await ComprasService.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<CompraDetalleHistoricoDto>(listadoEntidad));
        }

        /// <summary>Endpoint para reservar asientos mientras se realiza la compra.</summary>
        /// <param name="parametrosReservarAsientos">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista Dto con la información de los asientos reservados.</returns>
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [Route("ReservaAsientosVuelo")]
        public async Task<ActionResult<List<VueloAsiento>>> ReservaAsientosVuelo(ParamsReservarAsientos parametrosReservarAsientos)
        {
            IEnumerable<VueloAsiento> Entidad = await ComprasService.ReservarAsientosAsync(parametrosReservarAsientos);
            return Ok(_iMapper.Map<List<VueloAsiento>>(Entidad));
        }
        #endregion

    }
}