namespace Domain.General.CustomEntities.Compras
{
    /// <summary>Parámetros para reservar uno o varios asientos en un vuelo específico.</summary>
    public class ParamsReservarAsientos
    {
        /// <summary>Identificador del vuelo en el cual se reservan los asientos.</summary>
        public int VueloId { get; set; }

        /// <summary>Lista de asientos a reservar.</summary>
        public List<ParamsAsientosVuelos> ListaAsientoVuelo { get; set; } = new();
    }

    /// <summary>Representa cada asiento incluido en una reserva.</summary>
    public class ParamsAsientosVuelos
    {
        /// <summary>Identificador del asiento de vuelo que se desea reservar.</summary>
        public int VueloAsientoId { get; set; }

        /// <summary>Valor del RowVersion actual del asiento (para validar concurrencia).</summary>
        public byte[] RowVersion { get; set; }

        /// <summary>Fecha hasta la cual se bloquea el asiento temporalmente.</summary>
        public DateTime? BloqueadoHasta { get; set; }
    }
}
