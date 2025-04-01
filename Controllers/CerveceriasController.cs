using DozoWeb.Data;
using DozoWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DozoWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CerveceriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CerveceriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cervecerias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cerveceria>>> GetCervecerias(
            int page = 1,
            int pageSize = 10,
            string orderBy = "Nombre",
            string orderDirection = "asc")
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Los parámetros de paginación deben ser mayores a 0.");
            }

            // Construir la consulta inicial
            //var query = _context.Cervecerias.AsQueryable();

            // Construir la consulta inicial con Eager Loading
            var query = _context.Cervecerias
                .Include(c => c.Opiniones)
                .AsQueryable();

            // Aplicar ordenamiento
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = orderDirection.ToLower() == "desc"
                    ? query.OrderByDescending(c => EF.Property<object>(c, orderBy))
                    : query.OrderBy(c => EF.Property<object>(c, orderBy));
            }

            // Aplicar paginación
            var totalItems = await query.CountAsync();
            var cervecerias = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Crear una respuesta con metadatos
            var response = new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                Items = cervecerias
            };

            return Ok(response);
            //return await _context.Cervecerias.Include(cerveceria => cerveceria.Opiniones).ToListAsync();
        }

        // GET: api/Cervecerias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cerveceria>> GetCerveceria(int id)
        {
            var cerveceria = await _context.Cervecerias.FindAsync(id);

            if (cerveceria == null)
            {
                return NotFound();
            }

            return cerveceria;
        }

        //Filtrar cervecerias por rango de precios
        [HttpGet("FiltrarPorPrecio")]
        public async Task<ActionResult<IEnumerable<Cerveceria>>> FiltrarPorPrecio(decimal precioMin, decimal precioMax)
        {
            if (precioMin > precioMax)
            {
                return BadRequest("El precio mínimo no puede ser mayor que el precio máximo.");
            }

            var cerveceriasFiltradas = await _context.Cervecerias
                .Where(c => c.PrecioPromedio >= precioMin && c.PrecioPromedio <= precioMax)
                .ToListAsync();

            if (!cerveceriasFiltradas.Any())
            {
                return NotFound("No se encontraron cervecerías en el rango de precios especificado.");
            }

            return cerveceriasFiltradas;
        }

        // GET: api/Cervecerias/Buscar
        [HttpGet("Buscar")]
        public async Task<ActionResult<IEnumerable<Cerveceria>>> BuscarCervecerias(string? nombre = null, string? direccion = null)
        {
            // Verificar si al menos uno de los parámetros fue proporcionado
            if (string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(direccion))
            {
                return BadRequest("Debe proporcionar al menos un criterio de búsqueda (nombre o dirección).");
            }

            // Construir la consulta base
            var query = _context.Cervecerias.AsQueryable();

            // Agregar filtros dinámicos según los parámetros
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(c => c.Nombre.Contains(nombre));
            }

            if (!string.IsNullOrWhiteSpace(direccion))
            {
                query = query.Where(c => c.Direccion.Contains(direccion));
            }

            // Ejecutar la consulta
            var resultados = await query.ToListAsync();

            // Validar si no se encontraron resultados
            if (!resultados.Any())
            {
                return NotFound("No se encontraron cervecerías con los criterios especificados.");
            }

            return Ok(resultados);
        }

        /*
        private double GradosARadianes(double grados)
        {
            return grados * (Math.PI / 180);
        }
        */

        // GET: api/Cervecerias/BuscarPorUbicacion
        [HttpGet("BuscarPorUbicacion")]
        public async Task<ActionResult<IEnumerable<Cerveceria>>> BuscarPorUbicacion(
            double latitud,
            double longitud,
            double radioEnMetros)
        {
            // Validar entrada
            if (radioEnMetros <= 0)
            {
                return BadRequest("El radio debe ser mayor a 0.");
            }

            // Obtener todas las cervecerías
            var cervecerias = await _context.Cervecerias.ToListAsync();

            // Filtrar cervecerías dentro del radio
            var cerveceriasFiltradas = cervecerias.Where(c =>
                c.Latitud != null && c.Longitud != null &&
                CalcularDistancia(latitud, longitud, c.Latitud, c.Longitud) <= radioEnMetros
            ).ToList();

            if (!cerveceriasFiltradas.Any())
            {
                return NotFound("No se encontraron cervecerías en el área especificada.");
            }

            return Ok(cerveceriasFiltradas);
        }

        // Método para calcular la distancia entre dos puntos (Haversine Formula)
        private double CalcularDistancia(double latitud1, double longitud1, double? latitud2, double? longitud2)
        {
            if (latitud2 == null || longitud2 == null)
            {
                throw new ArgumentException("Las coordenadas no pueden ser nulas.");
            }

            const double RadioTierraEnKm = 6371.0;

            double lat1Rad = Math.PI * latitud1 / 180.0;
            double lat2Rad = Math.PI * latitud2.Value / 180.0;
            double deltaLat = Math.PI * (latitud2.Value - latitud1) / 180.0;
            double deltaLong = Math.PI * (longitud2.Value - longitud1) / 180.0;

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(deltaLong / 2) * Math.Sin(deltaLong / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return RadioTierraEnKm * c * 1000; // Convertir a metros
        }


        // POST: api/Cervecerias
        [HttpPost]
        public async Task<ActionResult<Cerveceria>> PostCerveceria(Cerveceria cerveceria)
        {
            // verificar si ya existe una cervecería con el mismo nombre y dirección
            var existe = await _context.Cervecerias.AnyAsync(c =>
                c.Nombre == cerveceria.Nombre &&
                c.Direccion == cerveceria.Direccion);
            if (existe)
            {
                return BadRequest("Ya existe una cervecería con el mismo nombre y dirección.");
            }

            // verificar si el precio promedio es razonable
            if (cerveceria.PrecioPromedio < 0 || cerveceria.PrecioPromedio > 1000)
            {
                return BadRequest("El precio promedio debe estar entre 0 y 1000.");
            }

            // Validación con Data Annotations
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Si todo es válido, guardar en la base de datos
            _context.Cervecerias.Add(cerveceria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCerveceria), new { id = cerveceria.Id }, cerveceria);
        }

        // PUT: api/Cervecerias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCerveceria(int id, Cerveceria cerveceria)
        {
            if (id != cerveceria.Id)
            {
                return BadRequest();
            }

            _context.Entry(cerveceria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CerveceriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Cervecerias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCerveceria(int id)
        {
            var cerveceria = await _context.Cervecerias.FindAsync(id);
            if (cerveceria == null)
            {
                return NotFound();
            }

            _context.Cervecerias.Remove(cerveceria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CerveceriaExists(int id)
        {
            return _context.Cervecerias.Any(e => e.Id == id);
        }
    }
}
