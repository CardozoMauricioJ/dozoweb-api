# API REST de DozoWeb: Gesti�n Integral de Cervecer�as y Opiniones con ASP.NET Core (Proyecto en producci�n)

# Descripci�n

Este proyecto implementa una API REST robusta y escalable utilizando ASP.NET Core para la gesti�n completa de la informaci�n de cervecer�as y las interacciones de los usuarios a trav�s de opiniones.

**Funcionalidades Clave:**

* **Gesti�n de Cervecer�as:** Permite la creaci�n, lectura, actualizaci�n y eliminaci�n (CRUD) de registros de cervecer�as, incluyendo detalles como nombre, direcci�n, ubicaci�n geogr�fica (latitud y longitud), y precio promedio.
* **B�squeda y Filtrado Avanzado:** Ofrece potentes capacidades de b�squeda por nombre y direcci�n, as� como filtrado por rango de precios y ubicaci�n geogr�fica (por radio y por �rea rectangular definida).
* **Gesti�n de Opiniones de Usuarios:** Facilita la recuperaci�n paginada de opiniones asociadas a cada cervecer�a.
* **Arquitectura RESTful:** Dise�o de endpoints intuitivo y los principios REST para una f�cil integraci�n con aplicaciones cliente.
* **Tecnolog�a:** Desarrollado con ASP.NET Core, Entity Framework Core para la gesti�n de la base de datos.

Esta API es la base para construir una experiencia de descubrimiento de cervecer�as rica e interactiva para los usuarios de DozoWeb.

## Tecnolog�as Utilizadas

* C# (.NET)
* ASP.NET Core
* Entity Framework Core
* Microsoft.EntityFrameworkCore
* System.ComponentModel.DataAnnotations
* SQL Server
* Swagger

## Instrucciones de Ejecuci�n

Instrucciones paso a paso para ejecutar el proyecto en un entorno local.

1.  Clonar el repositorio: `git clone https://github.com/CardozoMauricioJ/dozoweb-api.git`
2.  Navegar al directorio del proyecto: `cd dozoweb-api`
3.  Configurar la conexi�n a la base de datos SQL Server en `appsettings.json`.
4.  Ejecutar la aplicaci�n con .NET CLI: `dotnet run`
5.  La API estar� disponible en `http://localhost:7060`

## Endpoints de la API - Cervecer�as

Lista de los endpoints principales de la API para la gesti�n de cervecer�as.

* **GET /api/Cervecerias:** Obtiene la lista de todas las cervecer�as (con paginaci�n y ordenamiento).
* **GET /api/Cervecerias/{id}:** Obtiene una cervecer�a por ID.
* **GET /api/Cervecerias/FiltrarPorPrecio:** Filtra las cervecer�as por rango de precios.
* **GET /api/Cervecerias/Buscar:** Busca cervecer�as por nombre o direcci�n.
* **GET /api/Cervecerias/BuscarPorUbicacion:** Busca cervecer�as por ubicaci�n (latitud, longitud y radio).
* **POST /api/Cervecerias:** Crea una nueva cervecer�a.
* **PUT /api/Cervecerias/{id}:** Modifica una cervecer�a existente.
* **DELETE /api/Cervecerias/{id}:** Elimina una cervecer�a por ID.

## Endpoints de la API - Opiniones

Lista de los endpoints principales de la API para la gesti�n de opiniones.

* **GET /api/Opiniones:** Obtiene la lista de todas las opiniones.
* **GET /api/Opiniones/{id}:** Obtiene una opini�n por ID.
* **POST /api/Opiniones:** Crea una nueva opini�n.
* **PUT /api/Opiniones/{id}:** Modifica una opini�n existente.
* **DELETE /api/Opiniones/{id}:** Elimina una opini�n por ID.

## Modelo de datos - Cervecer�a

* **Id:** Clave primaria (entero).
* **Nombre:** Nombre de la cervecer�a (cadena, obligatorio, m�ximo 100 caracteres).
* **Direccion:** Direcci�n de la cervecer�a (cadena, obligatorio, m�ximo 200 caracteres).
* **PrecioPromedio:** Precio promedio de las cervezas (decimal, entre 0 y 1000).
* **Latitud:** Latitud de la ubicaci�n (doble, entre -90 y 90).
* **Longitud:** Longitud de la ubicaci�n (doble, entre -180 y 180).
* **Opiniones:** Lista de opiniones asociadas a la cervecer�a (relaci�n uno a muchos).

## Modelo de datos - Opini�n

* **Id:** Clave primaria (entero).
* **Usuario:** Nombre del usuario que deja la opini�n (cadena, obligatorio).
* **Puntaje:** Puntaje de la opini�n (entero, entre 1 y 5).
* **Comentario:** Comentario opcional (cadena).
* **Fecha:** Fecha de la opini�n (DateTime).
* **CerveceriaId:** ID de la cervecer�a asociada (entero, obligatorio).

## Autor

Desarrollado por Mauricio Javier Cardozo - mau.webapp@gmail.com - LinkedIn (https://www.linkedin.com/in/MauricioCardozo1).