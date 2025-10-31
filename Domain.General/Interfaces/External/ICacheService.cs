using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.General.Interfaces.External
{
    /// <summary>Define las operaciones básicas del servicio de caché con redis.</summary>
    public interface ICacheService
    {
        /// <summary>Obtiene un valor desde el caché según la clave especificada.</summary>
        /// <typeparam name="T">Tipo de objeto esperado.</typeparam>
        /// <param name="key">Clave única del objeto en caché.</param>
        /// <returns>Objeto deserializado o null si no existe.</returns>
        Task<T?> GetAsync<T>(string key);

        /// <summary>Almacena un valor en el caché con una posible expiración.</summary>
        /// <typeparam name="T">Tipo del objeto a almacenar.</typeparam>
        /// <param name="key">Clave única.</param>
        /// <param name="value">Valor a guardar.</param>
        /// <param name="expiration">Tiempo de expiración.</param>
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>Elimina un valor específico del caché.</summary>
        /// <param name="key">Clave única del valor a eliminar.</param>
        Task RemoveAsync(string key);
    }
}
