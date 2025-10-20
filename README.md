# MicroservicioGeneral

**Proyecto de gestión completa de vuelos y reservas de asientos**

Este proyecto se encarga de la parametrización del sistema, incluyendo la creación y gestión de **ciudades, países, aviones, asientos de aviones** y la **gestión completa de vuelos**. Los estados de un vuelo incluyen: **programado, disponible, cerrado, en embarque, despegado, aterrizado, finalizado o cancelado**.

Además, permite la **reserva temporal de asientos** mientras se completa la compra y la **compra final de los mismos**. Se implementa **concurrencia** para evitar la sobreventa de asientos de un vuelo, garantizando la consistencia de la información.

Gracias a la integración con el **microservicio de seguridad** y la implementación de **JWT**, se aplican **políticas de autorización** en los endpoints, asegurando que solo usuarios autenticados y autorizados puedan realizar ciertas acciones.

Adicionalmente, se está implementando **auditoría general de toda la aplicación**, registrando las acciones críticas, y métricas de **vuelos más buscados**, para análisis y optimización del sistema.

---

## Tecnologías utilizadas

- **Backend:** ASP.NET Core 8
- **Microservicio de seguridad:** JWT y políticas de autorización
- **Patrones de diseño:** Repository, Service Layer, DTO, Service Locator, Unit of Work
- **Arquitectura:** Arquitectura limpia, respetando principios SOLID
- **Documentación de API:** Swagger/OpenAPI para visualización y prueba de endpoints
- **Gestión de concurrencia:** Para evitar sobreventa de asientos
- **Auditoría y métricas:** Seguimiento de acciones críticas y análisis de vuelos más buscados
- **Dependencias externas:** Proyecto `Utilitarios` ([https://github.com/Brayan980312/Utilitarios](https://github.com/Brayan980312/Utilitarios))

---

## Requisitos previos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- **Visual Studio 2022**
- **.NET 8 SDK**
- **Proyecto `Utilitarios`** en la **misma raíz** que este proyecto
- Conexión a la base de datos configurada correctamente

---

## Configuración

1. Clona el repositorio:

```bash
   git clone https://github.com/Brayan980312/MicroservicioGeneral
```

2. Descarga también el proyecto de utilitarios:
   (Asegúrate de que ambos proyectos estén en la misma carpeta raíz.)

```bash
   git clone https://github.com/Brayan980312/Utilitarios
```

3. Abre el proyecto en Visual Studio 2022 y establece el proyecto API como proyecto de inicio.
4. Configura el archivo appsettings.json para tu entorno de base de datos y servicios externos

```bash
   {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
        "FlyHub": "Server=<SERVIDOR>;Database=<BASE_DATOS>;Persist Security Info=True;User ID=<USUARIO>;Password=<CLAVE>;MultipleActiveResultSets=True;App=EntityFramework;Encrypt=False"
  },
  "ExternalServices": {
    "BaseUrl": "https://localhost:7241/api/"
  }
}
```

5. Compila y ejecuta el proyecto desde Visual Studio 2022 preferiblemente.

---

## Funcionalidades principales

- **Parametrización del sistema:** Ciudades, países, aviones y asientos.
- **Gestión de vuelos:** Creación, actualización y cambios de estado de los vuelos.
- **Simulación de vuelos:** Actualización automática de estados de vuelo.
- **Reservas temporales de asientos:** Bloqueo temporal hasta completar la compra.
- **Compra final de asientos:** Confirmación de la transacción y liberación de bloqueos.
- **Concurrencia:** Prevención de sobreventa de asientos mediante control de accesos concurrentes.
- **Autorización:** Endpoints protegidos mediante JWT y políticas definidas por roles o permisos.
- **Auditoría general:** Registro de todas las acciones críticas en la aplicación.
- **Métricas de vuelos más buscados:** Seguimiento para análisis y optimización del sistema.

---

## Arquitectura del proyecto

El proyecto sigue **arquitectura limpia** con capas bien definidas:

- **API:** Controladores que exponen los endpoints
- **Domain:** Modelos de dominio y Data Transfer Objects, Lógica de negocio, validaciones y políticas de concurrencia
- **Infrastructure / Repository:** Acceso a base de datos
- **Utilitarios:** Funcionalidades compartidas (proyecto externo)
- **Patrones aplicados:** Repository, Service Layer, DTO, Service Locator, Unit of Work

---

## Buenas prácticas y recomendaciones

- Mantener actualizado el proyecto **Utilitarios** para evitar errores de compilación.
- Revisar siempre las rutas y configuraciones de **appsettings.json** antes de ejecutar.
- Validar la seguridad del **JWT Secret** y aplicar políticas de autorización correctamente.
- Garantizar la concurrencia en las operaciones de reserva y compra de asientos.
- Registrar auditoría de acciones y mantener métricas de vuelos más buscados actualizadas.
- Seguir los principios **SOLID** para cualquier nueva implementación.
- Documentar cualquier nuevo endpoint siguiendo la misma estructura.
