namespace Api.General.Controllers
{
    using AutoMapper;
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.DTOs.Administracion;
    using Domain.General.Entities;
    using Domain.General.Interfaces.General;
    using Domain.General.Interfaces.UnitOfWork;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controlador que maneja los endpoints relacionados con la administración del sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class AdministracionController : ControllerBase
    {
        #region Variables

        /// <summary>Inyección de Dependecias de servicios.</summary>
        private readonly IServiceUnitOfWork _iServiceUnitOfWork;

        /// <summary>Inyeccion de convertidor o resolutor de modelos.</summary>
        private readonly IMapper _iMapper;

        /// <summary>
        /// Obtiene una instancia del servicio de administración a través de la unidad de trabajo de servicios.
        /// </summary>
        private IAdministracionService AdministracionService => _iServiceUnitOfWork.GetService<IAdministracionService>();

        #endregion Variables

        #region Constructor

        /// <summary>Inicializa una nueva instancia de la clase AdministracionController.</summary>
        /// <param name="iMapper">Inyección de convertidor o resolutor de modelos.</param>
        /// <param name="iServiceUnitOfWork">Inyección de dependecias de Servicios.</param>
        public AdministracionController(IMapper iMapper, IServiceUnitOfWork iServiceUnitOfWork)
        {
            _iMapper = iMapper;
            _iServiceUnitOfWork = iServiceUnitOfWork;
        }

        #endregion

        #region EndPoints
        /// <summary>Endpoint para crear o actualizar la entidad.</summary>
        /// <param name="parametrosCrearActualizarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("CrearActualizarPais")]
        public async Task<ActionResult<List<PaisDto>>> CrearActualizarPais(ParamsCrearActualizarPais parametrosCrearActualizarEntidad)
        {
            IEnumerable<Pais> listadoEntidad = await AdministracionService.CreateAsync(parametrosCrearActualizarEntidad);
            return Ok(_iMapper.Map<List<PaisDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista de Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ConsultarPais")]
        public async Task<ActionResult<List<PaisDto>>> ConsultarPais([FromQuery] ParamsConsultarPais parametrosConsultarEntidad)
        {
            IEnumerable<Pais> listadoEntidad = await AdministracionService.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<PaisDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para crear o actualizar la entidad.</summary>
        /// <param name="parametrosCrearActualizarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("CrearActualizarCiudad")]
        public async Task<ActionResult<List<CiudadDto>>> CrearActualizarCiudad(ParamsCrearActualizarCiudad parametrosCrearActualizarEntidad)
        {
            IEnumerable<BusquedaCiudadPoco> listadoEntidad = await AdministracionService.CreateAsync(parametrosCrearActualizarEntidad);
            return Ok(_iMapper.Map<List<CiudadDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista de Dto con toda la entidad del sistema.</returns>
        [HttpGet("ConsultarCiudad")]
        public async Task<ActionResult<List<CiudadDto>>> ConsultarCiudad([FromQuery] ParamsConsultarCiudad parametrosConsultarEntidad)
        {
            IEnumerable<BusquedaCiudadPoco> listadoEntidad = await AdministracionService.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<CiudadDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista de Dto con toda la entidad del sistema.</returns>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet("ConsultarMetodoPago")]
        public async Task<ActionResult<List<MetodoPagoDto>>> ConsultarMetodoPago([FromQuery] ParamsConsultarMetodoPago parametrosConsultarEntidad)
        {
            IEnumerable<MetodoPago> listadoEntidad = await AdministracionService.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<MetodoPagoDto>>(listadoEntidad));
        }
        #endregion
    }
}