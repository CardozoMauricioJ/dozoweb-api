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

            // Calcular el total de páginas
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Agregado

            // Crear una respuesta con metadatos
            var response = new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                Items = cervecerias
            };

            return Ok(response);
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

        // GET: api/Cervecerias/BuscarCerveceriasEnRectangulo
        [HttpGet("BuscarCerveceriasEnRectangulo")]
        public async Task<ActionResult<IEnumerable<Cerveceria>>> BuscarCerveceriasEnRectangulo(
            double northEastLat,
            double northEastLng,
            double southWestLat,
            double southWestLng)
        {
            // Validar entrada
            if (northEastLat < southWestLat || northEastLng < southWestLng)
            {

                return BadRequest("Los límites del rectángulo no son válidos.");
            }

            var cerveceriasEnRectangulo = await _context.Cervecerias
                .Where(c => c.Latitud != null && c.Longitud != null &&
                            c.Latitud <= northEastLat && c.Latitud >= southWestLat &&
                            c.Longitud <= northEastLng && c.Longitud >= southWestLng)
                .Include(c => c.Opiniones)
                .ToListAsync();

            if (!cerveceriasEnRectangulo.Any())
            {
                return NotFound("No se encontraron cervecerías en el área especificada.");
            }

            return Ok(cerveceriasEnRectangulo);
        }

        // GET: api/Cervecerias/{CerveceriaId}/Opiniones
        [HttpGet("{CerveceriaId}/Opiniones")]
        public async Task<ActionResult<IEnumerable<Opinion>>> GetOpinionesPorCerveceria(
            int CerveceriaId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5)
        {
            if (pageSize <= 0 || pageSize > 50)
            {
                return BadRequest("El tamaño de página debe ser mayor que 0 y menor o igual a 50.");
            }

            var totalOpiniones = await _context.Opiniones
                .Where(o => o.CerveceriaId == CerveceriaId)
                .CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalOpiniones / pageSize);

            if (pageNumber < 1 || pageNumber > totalPages && totalPages > 0)
            {
                return NotFound("Página no encontrada.");
            }

            var opiniones = await _context.Opiniones
                .Where(o => o.CerveceriaId == CerveceriaId)
                .OrderByDescending(o => o.Fecha)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (opiniones == null || !opiniones.Any())
            {
                return Ok(new List<Opinion>());
            }

            Response.Headers["X-Total-Count"] = totalOpiniones.ToString();
            Response.Headers["X-Total-Pages"] = totalPages.ToString();
            Response.Headers["X-Current-Page"] = pageNumber.ToString();
            Response.Headers["X-Page-Size"] = pageSize.ToString();

            return Ok(opiniones);
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
