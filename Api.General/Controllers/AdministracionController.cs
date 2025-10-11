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

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class AdministracionController : ControllerBase
    {
        #region Variables

        /// <summary>Inyección de Dependecias de servicios.</summary>
        private readonly IServiceUnitOfWork _iServiceUnitOfWork;

        /// <summary>Inyeccion de convertidor o resolutor de modelos.</summary>
        private readonly IMapper _iMapper;

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

        /// <summary>Endpoint para crear o actualizar la entidad.</summary>
        /// <param name="parametrosCrearActualizarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista Dto con toda la entidad del sistema.</returns>
        [HttpPost]
        [Route("CrearActualizarPais")]
        public async Task<ActionResult<List<PaisDto>>> CrearActualizarPais(ParamsCrearActualizarPais parametrosCrearActualizarEntidad)
        {
            IAdministracionService Service = _iServiceUnitOfWork.GetService<IAdministracionService>();

            IEnumerable<Pais> listadoEntidad = await Service.CreateAsync(parametrosCrearActualizarEntidad);
            return Ok(_iMapper.Map<List<PaisDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista de Dto con toda la entidad del sistema.</returns>
        [HttpGet("ConsultarPais")]
        public async Task<ActionResult<List<PaisDto>>> ConsultarPais([FromQuery] ParamsConsultarPais parametrosConsultarEntidad)
        {
            IAdministracionService Service = _iServiceUnitOfWork.GetService<IAdministracionService>();

            IEnumerable<Pais> listadoEntidad = await Service.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<PaisDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para crear o actualizar la entidad.</summary>
        /// <param name="parametrosCrearActualizarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista Dto con toda la entidad del sistema.</returns>
        [HttpPost]
        [Route("CrearActualizarCiudad")]
        public async Task<ActionResult<List<CiudadDto>>> CrearActualizarCiudad(ParamsCrearActualizarCiudad parametrosCrearActualizarEntidad)
        {
            IAdministracionService Service = _iServiceUnitOfWork.GetService<IAdministracionService>();

            IEnumerable<Ciudad> listadoEntidad = await Service.CreateAsync(parametrosCrearActualizarEntidad);
            return Ok(_iMapper.Map<List<CiudadDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista de Dto con toda la entidad del sistema.</returns>
        [HttpGet("ConsultarCiudad")]
        public async Task<ActionResult<List<CiudadDto>>> ConsultarCiudad([FromQuery] ParamsConsultarCiudad parametrosConsultarEntidad)
        {
            IAdministracionService Service = _iServiceUnitOfWork.GetService<IAdministracionService>();

            IEnumerable<Ciudad> listadoEntidad = await Service.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<CiudadDto>>(listadoEntidad));
        }
    

        /// <summary>Endpoint para crear o actualizar la entidad.</summary>
        /// <param name="parametrosCrearActualizarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista Dto con toda la entidad del sistema.</returns>
        [HttpPost]
        [Route("CrearActualizarMetodoPago")]
        public async Task<ActionResult<List<MetodoPagoDto>>> CrearActualizarMetodoPago(ParamsCrearActualizarMetodoPago parametrosCrearActualizarEntidad)
        {
            IAdministracionService Service = _iServiceUnitOfWork.GetService<IAdministracionService>();

            IEnumerable<MetodoPago> listadoEntidad = await Service.CreateAsync(parametrosCrearActualizarEntidad);
            return Ok(_iMapper.Map<List<MetodoPagoDto>>(listadoEntidad));
        }

        /// <summary>Endpoint para consultar la información de la entidad registrada en el sistema.</summary>
        /// <param name="parametrosConsultarEntidad">Parametros de entrada para realizar la operacion.</param>
        /// <returns>Lista de Dto con toda la entidad del sistema.</returns>
        [HttpGet("ConsultarMetodoPago")]
        public async Task<ActionResult<List<MetodoPagoDto>>> ConsultarMetodoPago([FromQuery] ParamsConsultarMetodoPago parametrosConsultarEntidad)
        {
            IAdministracionService Service = _iServiceUnitOfWork.GetService<IAdministracionService>();

            IEnumerable<MetodoPago> listadoEntidad = await Service.GetWithParamsAsync(parametrosConsultarEntidad);
            return Ok(_iMapper.Map<List<MetodoPagoDto>>(listadoEntidad));
        }
    }
}