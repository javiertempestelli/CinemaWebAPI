using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCinema;
using Application.DTOs;
using Infrastructure.Queries;

namespace CinemaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionesController : ControllerBase
    {
        private readonly CinemaContext _context;

        public FuncionesController(CinemaContext context)
        {
            _context = context;
        }

        // GET: api/Funciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuncionDTO>>> GetFunciones()
        {
            var funciones = await _context.Funciones
                .Select(f => new FuncionDTO
                {
                    SalaId = f.SalaId,
                    Fecha = f.Fecha,
                    Horario = f.Horario,
                    PeliculaId = f.PeliculaId
                })
                .ToListAsync();

            return funciones;
        }

        // GET: api/Funciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionDTO>> GetFuncion(int id)
        {
            var funcion = await _context.Funciones.FindAsync(id);

            if (funcion == null)
            {
                return NotFound();
            }

            var funcionDTO = new FuncionDTO
            {
                FuncionId = funcion.FuncionId,
                // Mapea otros campos de Funcion a FuncionDTO según tus necesidades.
            };

            return funcionDTO;
        }

        // PUT: api/Funciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFuncion(int id, FuncionDTO funcionDTO)
        {
            if (id != funcionDTO.FuncionId)
            {
                return BadRequest();
            }

            var funcion = await _context.Funciones.FindAsync(id);
            if (funcion == null)
            {
                return NotFound();
            }

            // Actualiza los campos de la entidad Funcion con los valores de funcionDTO.
            // Puedes usar AutoMapper u otras técnicas de mapeo aquí.

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionExists(id))
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

        [HttpGet("{id}/tickets")]
        public async Task<ActionResult<CantidadTicketsResponse>> GetCantidadTickets(int id)
        {
            var funcion = await _context.Funciones.FindAsync(id);

            if (funcion == null)
            {
                return NotFound(); // La función con el ID proporcionado no existe.
            }

            int cantidadDisponible = funcion.Sala.Capacidad - await _context.Tickets.CountAsync(t => t.FuncionId == id);

            var cantidadTicketsResponse = new CantidadTicketsResponse
            {
                Cantidad = cantidadDisponible
            };

            return cantidadTicketsResponse;
        }
        // POST: api/Funciones
        [HttpPost]
        public async Task<ActionResult<FuncionDTO>> PostFuncion(FuncionDTO funcionDTO)
        {
            if (funcionDTO == null)
            {
                return BadRequest("El objeto funcionDTO es nulo.");
            }

            // Mapea los campos de funcionDTO a la entidad Funcion.
            var funcion = new Funcion
            {
                SalaId = funcionDTO.SalaId,
                Fecha = DateTime.Parse(funcionDTO.Fecha.ToString("yyyy-MM-dd")),
                Horario = DateTime.Parse(funcionDTO.Horario.ToString("HH:mm:ss")),
                PeliculaId = funcionDTO.PeliculaId
                // Asigna otros campos según corresponda.
            };

            // Agrega la nueva Funcion a la base de datos.
            _context.Funciones.Add(funcion);
            await _context.SaveChangesAsync();

            // Puedes mapear la entidad Funcion nuevamente a FuncionDTO si es necesario.
            // Esto dependerá de tu lógica de negocio y de si deseas devolver algún dato adicional.

            return CreatedAtAction("GetFuncion", new { id = funcion.FuncionId }, funcionDTO);
        }

        // DELETE: api/Funciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncion(int id)
        {
            var funcion = await _context.Funciones.FindAsync(id);
            if (funcion == null)
            {
                return NotFound();
            }

            _context.Funciones.Remove(funcion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FuncionExists(int id)
        {
            return _context.Funciones.Any(e => e.FuncionId == id);
        }
    }
}

