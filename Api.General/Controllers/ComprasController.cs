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

        /// <summary>Endpoint para realizar la compra de los asientos de un vuelo.</summary>
        /// <param name="parametrosComprarAsientos">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista Dto con la información de la compra realizada.</returns>
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [Route("CompraAsientosVuelo")]
        public async Task<ActionResult<Compra>> CompraAsientosVuelo(ParamsCompraAsientos parametrosComprarAsientos)
        {
            var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");
            Compra Entidad = await ComprasService.ComprarAsientosAsync(parametrosComprarAsientos, Convert.ToInt32(usuarioIdClaim!.Value));
            return Ok(_iMapper.Map<Compra>(Entidad));
        }

        /// <summary>Endpoint para consultar la información de las compras realizadas por el usuario.</summary>
        /// <returns>Objeto de Dto con la información de las compras realizadas por el usuario.</returns>
        [Authorize(Roles = "Cliente")]
        [HttpGet("BuscarComprasUsuario")]
        public async Task<ActionResult<List<ComprasRealizadasUsuarioDto>>> BuscarComprasUsuario()
        {
            var usuarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");
            IEnumerable<ComprasRealizadasUsuarioPoco> listadoEntidad = await ComprasService.BuscarComprasUsuarioAsycn(Convert.ToInt32(usuarioIdClaim!.Value));
            return Ok(_iMapper.Map<List<ComprasRealizadasUsuarioDto>>(listadoEntidad));
        }
        #endregion

    }
}