using DozoWeb.Data;
using DozoWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DozoWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpinionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OpinionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Opiniones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Opinion>>> GetOpiniones()
        {
            return await _context.Opiniones.ToListAsync();
        }

        // GET: api/Opiniones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Opinion>> GetOpinion(int id)
        {
            var opinion = await _context.Opiniones.FindAsync(id);

            if (opinion == null)
            {
                return NotFound();
            }

            return opinion;
        }

        // POST: api/Opiniones
        [HttpPost]
        public async Task<ActionResult<Opinion>> PostOpinion(Opinion opinion)
        {
            // Verificar si el modelo es válido
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Aquí se imprimen los errores
                }
                return BadRequest(ModelState); // Esto devolverá detalles de qué parte del modelo no es válida
            }

            // Validaciones básicas
            if (opinion.Puntaje < 1 || opinion.Puntaje > 5)
            {
                return BadRequest("El puntaje debe estar entre 1 y 5.");
            }

            // Validación de la cervecería
            var cerveceria = await _context.Cervecerias.FindAsync(opinion.CerveceriaId);
            if (cerveceria == null)
            {
                return BadRequest("La cervecería especificada no existe.");
            }

            opinion.Fecha = DateTime.Now;
            _context.Opiniones.Add(opinion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOpinion), new { id = opinion.Id }, opinion);
        }

        // PUT: api/Opiniones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOpinion(int id, Opinion opinion)
        {
            if (id != opinion.Id)
            {
                return BadRequest();
            }

            _context.Entry(opinion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpinionExists(id))
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

        // DELETE: api/Opiniones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOpinion(int id)
        {
            var opinion = await _context.Opiniones.FindAsync(id);
            if (opinion == null)
            {
                return NotFound();
            }

            _context.Opiniones.Remove(opinion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OpinionExists(int id)
        {
            return _context.Opiniones.Any(e => e.Id == id);
        }
    }
}

