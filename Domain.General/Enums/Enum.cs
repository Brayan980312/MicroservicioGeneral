namespace Domain.General.Enums
{
    public class Enum
    {
        /// <summary>
        /// Define los codigos de respuesta de la API.
        /// </summary>
        public enum RespuestaAPI
        {
            Ok = 200,
            Validacion = 422,
            Error = 500
        }

        /// <summary>
        /// Estados de un vuelo.
        /// </summary>
        public enum EstadoVuelo
        {
            Programado = 1,
            Disponible = 2,
            Cerrado = 3,
            EnEmbarque = 4,
            Despegado = 5,
            Aterrizado = 6,
            Finalizado = 7,
            Cancelado = 8,
        }

        /// <summary>
        /// Estados de la compra de un vuelo.
        /// </summary>
        public enum EstadoCompra
        {
            Comprado = 1,
            Embolsado = 2,
            Cancelado = 3
        }
    }
}
