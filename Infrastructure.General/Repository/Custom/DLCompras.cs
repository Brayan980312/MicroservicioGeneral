namespace Infrastructure.General.Repository.Custom
{
    using Domain.General.CustomEntities.Compras;
    using Domain.General.CustomEntities.Vuelo;
    using Domain.General.Entities;
    using Domain.General.Interfaces.Repository;
    using Infrastructure.General.Persistence.Context;
    using Microsoft.EntityFrameworkCore;
    using Utilitarios.Data;
    public class DLCompras: CrudSqlRepositorio<Compra>, IDLCompras
    {
        #region Variables

        /// <summary>Implementacion contexto.</summary>
        private readonly GeneralContext _GeneralContext;
        #endregion

        #region Constructor

        /// <summary>Inicializa la clase DLCompras.</summary>
        /// <param name="GeneralContext">Contexto bd.</param>
        public DLCompras(GeneralContext GeneralContext) : base(GeneralContext)
        {
            this._GeneralContext = GeneralContext;
        }

        #endregion

        /// <summary>Consulta personalizada que obtiene las compras realizadas por el usuario que realiza la petición.</summary>
        /// <param name="userId">Usuario que realiza la petición.</param>
        /// <returns>Lista ComprasRealizadasUsuarioPoco que cumplen con los filtros de búsqueda.</returns>
        public async Task<IEnumerable<ComprasRealizadasUsuarioPoco>> BuscarComprasUsuarioAsycn(int userId)
        {
            object[] variables = { "@UsuarioId" };
            object[] parametros = { userId };

            return await ConsultarStoreProcedureAsync<ComprasRealizadasUsuarioPoco>("[Compras].[ConsultaComprasYDetalle]", variables, parametros);
        }
    }
}
