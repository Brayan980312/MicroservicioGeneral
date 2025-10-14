namespace Domain.General.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Utilitarios.Entities;

    public class VueloAsiento : EntidadBase
    {
        /// <summary>Identificador único del registro del asiento asignado a un vuelo.</summary>
        public int VueloAsientoId { get; set; }

        /// <summary>Identificador del vuelo al que pertenece el asiento.</summary>
        public int VueloId { get; set; }

        /// <summary>Identificador del asiento físico dentro del avión.</summary>
        public int AsientoId { get; set; }

        /// <summary>Indica si el asiento está reservado temporalmente.</summary>
        public bool VueloAsientoReservado { get; set; }

        /// <summary>Indica si el asiento ya fue comprado definitivamente.</summary>
        public bool VueloAsientoComprado { get; set; }

        /// <summary>
        /// Fecha y hora hasta la cual el asiento permanecerá bloqueado para evitar su compra simultánea.
        /// Se utiliza como mecanismo de concurrencia temporal.
        /// </summary>
        public DateTime? VueloAsientoBloqueadoHasta { get; set; }

        /// <summary>
        /// Campo de versión de fila (rowversion) para el manejo de concurrencia optimista.
        /// Se actualiza automáticamente por SQL Server en cada modificación.
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}