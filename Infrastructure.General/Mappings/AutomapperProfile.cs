namespace Infrastructure.General.Mappings
{
    using AutoMapper;
    using Domain.General.CustomEntities.Params;
    using Domain.General.Entities;
    using Domain.General.DTOs.Auditoria;
    using Domain.General.DTOs.Parametros;
    using Domain.General.CustomEntities.Auditoria;
    using Domain.General.DTOs.Administracion;
    using Domain.General.CustomEntities.Administracion;
    using Domain.General.DTOs.Creditos;
    using Domain.General.CustomEntities.Creditos;
    using Domain.General.DTOs.Avion;
    using Domain.General.CustomEntities.Avion;
    using Domain.General.DTOs.Vuelo;
    using Domain.General.CustomEntities.Vuelo;


    /// <summary>Configura los mapeos entre las entidades de dominio general y los objetos de transferencia de datos (DTOs/Params) mediante AutoMapper.
    /// Permite la conversión bidireccional entre los tipos configurados.
    /// </summary>
    public class AutomapperProfile : Profile
    {

        /// <summary>Inicializa una nueva instancia de <see cref="AutomapperProfile"/> y configura los mapeos entre entidades y DTOs/Params.</summary>
        public AutomapperProfile()
        {
            /// <summary>Mapea la entidad <see cref="Auditoria"/> a <see cref="ParamsAuditoria"/> y viceversa.</summary>
            CreateMap<Auditoria, ParamsAuditoria>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Auditoria"/> a <see cref="AuditoriaDto"/> y viceversa.</summary>
            CreateMap<Auditoria, AuditoriaDto>().ReverseMap();

            /// <summary>Mapea la entidad <see cref="Parametros"/> a <see cref="ParametrosDto"/> y viceversa.</summary>
            CreateMap<Parametros, ParametrosDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Parametros"/> a <see cref="ParamsActualizarParametros"/> y viceversa.</summary>
            CreateMap<Parametros, ParamsActualizarParametros>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Parametros"/> a <see cref="ParamsConsultarParametros"/> y viceversa.</summary>
            CreateMap<Parametros, ParamsConsultarParametros>().ReverseMap();

            /// <summary>Mapea la entidad <see cref="Pais"/> a <see cref="PaisDto"/> y viceversa.</summary>
            CreateMap<Pais, PaisDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Pais"/> a <see cref="ParamsCrearActualizarPais"/> y viceversa.</summary>
            CreateMap<Pais, ParamsCrearActualizarPais>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Pais"/> a <see cref="ParamsConsultarPais"/> y viceversa.</summary>
            CreateMap<Pais, ParamsConsultarPais>().ReverseMap();

            /// <summary>Mapea la entidad <see cref="Ciudad"/> a <see cref="CiudadDto"/> y viceversa.</summary>
            CreateMap<Ciudad, CiudadDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Ciudad"/> a <see cref="ParamsCrearActualizarCiudad"/> y viceversa.</summary>
            CreateMap<Ciudad, ParamsCrearActualizarCiudad>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Ciudad"/> a <see cref="ParamsConsultarCiudad"/> y viceversa.</summary>
            CreateMap<Ciudad, ParamsConsultarCiudad>().ReverseMap();

            /// <summary>Mapea la entidad <see cref="MetodoPago"/> a <see cref="MetodoPagoDto"/> y viceversa.</summary>
            CreateMap<MetodoPago, MetodoPagoDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="MetodoPago"/> a <see cref="ParamsCrearActualizarMetodoPago"/> y viceversa.</summary>
            CreateMap<MetodoPago, ParamsCrearActualizarMetodoPago>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="MetodoPago"/> a <see cref="ParamsConsultarMetodoPago"/> y viceversa.</summary>
            CreateMap<MetodoPago, ParamsConsultarMetodoPago>().ReverseMap();

            /// <summary>Mapea la entidad <see cref="CreditoUsuario"/> a <see cref="CreditoUsuarioDto"/> y viceversa.</summary>
            CreateMap<CreditoUsuario, CreditoUsuarioDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="CreditoUsuario"/> a <see cref="ParamsCrearActualizarCreditoUsuario"/> y viceversa.</summary>
            CreateMap<CreditoUsuario, ParamsCrearActualizarCreditoUsuario>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="CreditoUsuario"/> a <see cref="ParamsConsultarCreditoUsuario"/> y viceversa.</summary>
            CreateMap<CreditoUsuario, ParamsConsultarCreditoUsuario>().ReverseMap();

            /// <summary>Mapea la entidad <see cref="Avion"/> a <see cref="AvionDto"/> y viceversa.</summary>
            CreateMap<Avion, AvionDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Avion"/> a <see cref="ParamsCrearActualizarAvion"/> y viceversa.</summary>
            CreateMap<Avion, ParamsCrearActualizarAvion>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Avion"/> a <see cref="ParamsConsultarAvion"/> y viceversa.</summary>
            CreateMap<Avion, ParamsConsultarAvion>().ReverseMap();

            /// <summary>Mapea la entidad <see cref="AsientoAvion"/> a <see cref="AsientoAvionDto"/> y viceversa.</summary>
            CreateMap<AsientoAvion, AsientoAvionDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="AsientoAvion"/> a <see cref="ParamsCrearActualizarAsientoAvion"/> y viceversa.</summary>
            CreateMap<AsientoAvion, ParamsCrearActualizarAsientoAvion>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="AsientoAvion"/> a <see cref="ParamsConsultarAsientoAvion"/> y viceversa.</summary>
            CreateMap<AsientoAvion, ParamsConsultarAsientoAvion>().ReverseMap();




            /// <summary>Mapea la entidad <see cref="Vuelo"/> a <see cref="VueloDto"/> y viceversa.</summary>
            CreateMap<Vuelo, VueloDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Vuelo"/> a <see cref="ParamsCrearActualizarVuelo"/> y viceversa.</summary>
            CreateMap<Vuelo, ParamsCrearActualizarVuelo>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Vuelo"/> a <see cref="ParamsConsultarVuelo"/> y viceversa.</summary>
            CreateMap<Vuelo, ParamsConsultarVuelo>().ReverseMap();

            /// <summary>Mapea la entidad <see cref="VueloHistorico"/> a <see cref="VueloHistoricoDto"/> y viceversa.</summary>
            CreateMap<VueloHistorico, VueloHistoricoDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="VueloHistorico"/> a <see cref="ParamsCrearVueloHistorico"/> y viceversa.</summary>
            CreateMap<VueloHistorico, ParamsCrearVueloHistorico>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="VueloHistorico"/> a <see cref="ParamsConsultarVueloHistorico"/> y viceversa.</summary>
            CreateMap<VueloHistorico, ParamsConsultarVueloHistorico>().ReverseMap();

            /// <summary>Mapea la entidad <see cref="Compra"/> a <see cref="CompraDto"/> y viceversa.</summary>
            CreateMap<Compra, CompraDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Compra"/> a <see cref="ParamsCrearActualizarCompra"/> y viceversa.</summary>
            CreateMap<Compra, ParamsCrearActualizarCompra>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Compra"/> a <see cref="ParamsConsultarCompra"/> y viceversa.</summary>
            CreateMap<Compra, ParamsConsultarCompra>().ReverseMap();

            /// <summary>Mapea la entidad <see cref="CompraHistorico"/> a <see cref="CompraHistoricoDto"/> y viceversa.</summary>
            CreateMap<CompraHistorico, CompraHistoricoDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Compra"/> a <see cref="ParamsConsultarCompra"/> y viceversa.</summary>
            CreateMap<CompraHistorico, ParamsConsultarCompraHistorico>().ReverseMap();

            /// <summary>Mapea la entidad <see cref="CompraDetalle"/> a <see cref="CompraDetalleDto"/> y viceversa.</summary>
            CreateMap<CompraDetalle, CompraDetalleDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="CompraDetalle"/> a <see cref="ParamsCrearActualizarCompraDetalle"/> y viceversa.</summary>
            CreateMap<CompraDetalle, ParamsCrearActualizarCompraDetalle>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="CompraDetalle"/> a <see cref="ParamsConsultarCompraDetalle"/> y viceversa.</summary>
            CreateMap<CompraDetalle, ParamsConsultarCompraDetalle>().ReverseMap();

            /// <summary>Mapea la entidad <see cref="CompraDetalleHistorico"/> a <see cref="CompraDetalleHistoricoDto"/> y viceversa.</summary>
            CreateMap<CompraDetalleHistorico, CompraDetalleHistoricoDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="CompraDetalleHistorico"/> a <see cref="ParamsConsultarCompraDetalleHistorico"/> y viceversa.</summary>
            CreateMap<CompraDetalleHistorico, ParamsConsultarCompraDetalleHistorico>().ReverseMap();

        }
    }
}
