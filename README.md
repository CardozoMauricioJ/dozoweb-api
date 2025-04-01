# DozoWeb API REST - Cervecerías (Proyecto en producción)

## Descripción

Este proyecto es una API REST desarrollada con ASP.NET Core para gestionar la información de cervecerías y las opiniones de los usuarios. Permite crear, editar, eliminar y buscar cervecerías, filtrar por precio y ubicación, y gestionar las opiniones de los usuarios.

## Tecnologías Utilizadas

* C# (.NET)
* ASP.NET Core
* Entity Framework Core
* Microsoft.EntityFrameworkCore
* System.ComponentModel.DataAnnotations
* SQL Server
* Swagger

## Instrucciones de Ejecución

Instrucciones paso a paso para ejecutar el proyecto en un entorno local.

1.  Clonar el repositorio: `git clone https://github.com/CardozoMauricioJ/dozoweb-api.git`
2.  Navegar al directorio del proyecto: `cd dozoweb-api`
3.  Configurar la conexión a la base de datos SQL Server en `appsettings.json`.
4.  Ejecutar la aplicación con .NET CLI: `dotnet run`
5.  La API estará disponible en `http://localhost:7060`

## Endpoints de la API - Cervecerías

Lista de los endpoints principales de la API para la gestión de cervecerías.

* **GET /api/Cervecerias:** Obtiene la lista de todas las cervecerías (con paginación y ordenamiento).
* **GET /api/Cervecerias/{id}:** Obtiene una cervecería por ID.
* **GET /api/Cervecerias/FiltrarPorPrecio:** Filtra las cervecerías por rango de precios.
* **GET /api/Cervecerias/Buscar:** Busca cervecerías por nombre o dirección.
* **GET /api/Cervecerias/BuscarPorUbicacion:** Busca cervecerías por ubicación (latitud, longitud y radio).
* **POST /api/Cervecerias:** Crea una nueva cervecería.
* **PUT /api/Cervecerias/{id}:** Modifica una cervecería existente.
* **DELETE /api/Cervecerias/{id}:** Elimina una cervecería por ID.

## Endpoints de la API - Opiniones

Lista de los endpoints principales de la API para la gestión de opiniones.

* **GET /api/Opiniones:** Obtiene la lista de todas las opiniones.
* **GET /api/Opiniones/{id}:** Obtiene una opinión por ID.
* **POST /api/Opiniones:** Crea una nueva opinión.
* **PUT /api/Opiniones/{id}:** Modifica una opinión existente.
* **DELETE /api/Opiniones/{id}:** Elimina una opinión por ID.

## Modelo de datos - Cervecería

* **Id:** Clave primaria (entero).
* **Nombre:** Nombre de la cervecería (cadena, obligatorio, máximo 100 caracteres).
* **Direccion:** Dirección de la cervecería (cadena, obligatorio, máximo 200 caracteres).
* **PrecioPromedio:** Precio promedio de las cervezas (decimal, entre 0 y 1000).
* **Latitud:** Latitud de la ubicación (doble, entre -90 y 90).
* **Longitud:** Longitud de la ubicación (doble, entre -180 y 180).
* **Opiniones:** Lista de opiniones asociadas a la cervecería (relación uno a muchos).

## Modelo de datos - Opinión

* **Id:** Clave primaria (entero).
* **Usuario:** Nombre del usuario que deja la opinión (cadena, obligatorio).
* **Puntaje:** Puntaje de la opinión (entero, entre 1 y 5).
* **Comentario:** Comentario opcional (cadena).
* **Fecha:** Fecha de la opinión (DateTime).
* **CerveceriaId:** ID de la cervecería asociada (entero, obligatorio).

## Autor

Desarrollado por Mauricio Javier Cardozo - mau.webapp@gmail.com - LinkedIn (https://www.linkedin.com/in/MauricioCardozo1).