using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCinema;
using Infrastructure.Queries;
using Application.DTOs.Funcion;
using Application.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<ActionResult<IEnumerable<FuncionGETALL>>> GetFunciones()
        {

            var funciones = await _context.Funciones
                .Include(f => f.Sala) // Cargar la relación con la tabla Sala
                .Include(f => f.Pelicula) // Cargar la relación con la tabla Película
            .Select(f => new FuncionGETALL
            {
                    FuncionId = f.FuncionId,
                    SalaNombre= f.Sala.Nombre,
                    Fecha = f.Fecha,
                    Horario = f.Horario,
                    PeliculaTitulo = f.Pelicula.Titulo
            })
                .ToListAsync();

            return funciones;
        }


        // GET: api/Funciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionGET>> GetFuncion(int id)
        {
            var funcion = await _context.Funciones.FindAsync(id);

            if (funcion == null)
            {
                return NotFound();
            }
            var sala = await _context.Salas.FindAsync(funcion.SalaId);
            var pelicula = await _context.Peliculas.FindAsync(funcion.PeliculaId);
            var funcionGET = new FuncionGET
            {
                SalaNombre = sala.Nombre,
                Fecha = funcion.Fecha,
                Horario = funcion.Horario,
                PeliculaTitulo = pelicula.Titulo
            };

            return funcionGET;
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
            var sala = await _context.Salas.FindAsync(funcion.SalaId);
            int cantidadTicketsPorFuncion = await _context.Tickets.CountAsync(t => t.FuncionId == id);
            int cantidadDisponible = sala.Capacidad - cantidadTicketsPorFuncion;

            var cantidadTicketsResponse = new CantidadTicketsResponse
            {
                CantidadDisponible = cantidadDisponible
            };

            return cantidadTicketsResponse;
        }

        [HttpPost]
        public async Task<ActionResult<FuncionDTO>> PostFuncion(FuncionPOST funcionPOST)
        {
            // Verifica si hay funciones existentes en la misma sala que se superponen en tiempo
            DateTime nuevaFuncionInicio = funcionPOST.Horario; // Corrección aquí
            DateTime nuevaFuncionFin = nuevaFuncionInicio.AddMinutes(150); // 2 horas y 30 minutos

            bool superposicion = await _context.Funciones
                .AnyAsync(f =>
                    f.SalaId == funcionPOST.SalaId &&
                    f.Fecha.Date == funcionPOST.Fecha.Date && // Mismo día
                    f.Horario <= nuevaFuncionFin && f.Horario.AddMinutes(150) >= nuevaFuncionInicio); // Superposición de horarios

            if (superposicion)
            {
                return BadRequest("La nueva función se superpone con otra función en la misma sala.");
            }

            var funcion = new Funcion
            {
                SalaId = funcionPOST.SalaId,
                Fecha = funcionPOST.Fecha,
                Horario = funcionPOST.Horario,
                PeliculaId = funcionPOST.PeliculaId
            };

            _context.Funciones.Add(funcion);
            await _context.SaveChangesAsync();

            // Devuelve la función creada
            var funcionDTO = new FuncionDTO
            {
                SalaId = funcion.SalaId,
                Fecha = funcion.Fecha,
                Horario = funcion.Horario,
                PeliculaId = funcion.PeliculaId
            };

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

            // Verificar si hay tickets vendidos para esta función
            var ticketsVendidos = await _context.Tickets.AnyAsync(t => t.FuncionId == id);

            if (ticketsVendidos)
            {
                return BadRequest("No se puede eliminar la función porque tiene tickets vendidos.");
            }

            _context.Funciones.Remove(funcion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("disponibles")]
        public async Task<ActionResult<IEnumerable<FuncionDTO>>> GetFuncionesDisponibles(
            [FromQuery] DateTime? fecha,
            [FromQuery] int? peliculaId,
            [FromQuery] int? generoId)
        {
            var query = _context.Funciones.AsQueryable();

            // Aplica los filtros según los parámetros proporcionados.
            if (fecha.HasValue)
            {
                // Filtra por fecha.
                query = query.Where(f => f.Fecha.Date == fecha.Value.Date);
            }

            if (peliculaId.HasValue)
            {
                // Filtra por ID de película.
                query = query.Where(f => f.PeliculaId == peliculaId.Value);
            }

            if (generoId.HasValue)
            {
                // Filtra por género de película.
                query = query.Where(f => f.Pelicula.GeneroId == generoId.Value);
            }

            // Proyecta el resultado en FuncionDTO u otro DTO personalizado si es necesario.
            var funcionesDTO = await query
                .Select(f => new FuncionDTO
                {
                    SalaId = f.SalaId,
                    Fecha = f.Fecha,
                    Horario = f.Horario,
                    PeliculaId = f.PeliculaId
                })
                .ToListAsync();

            if (funcionesDTO.Count == 0)
            {
                return NotFound("No existen funciones disponibles para los criterios solicitados.");
            }
            return funcionesDTO;
        }



        private bool FuncionExists(int id)
        {
            return _context.Funciones.Any(e => e.FuncionId == id);
        }
    }
}

