using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Queries;
using CinemaWebAPI.Application.DTOs;
using Application.DTOs;

namespace CinemaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionController : ControllerBase
    {
        private readonly CinemaContext _context;

        public FuncionController(CinemaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuncionResponse>>> GetFuncionesDisponibles(
            [FromQuery] DateTime? fecha,
            [FromQuery] int? peliculaId,
            [FromQuery] int? generoId)
        {
            var query = _context.Funciones.AsQueryable();

            // Aplica los filtros opcionales.
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

            var funciones = await query
                .Include(f => f.Sala)
                .Include(f => f.Pelicula)
                .Select(f => new FuncionResponse
                {
                    FuncionId = f.FuncionId,
                    Pelicula = new PeliculaResponse
                    {
                        PeliculaId = f.Pelicula.PeliculaId,
                        Titulo = f.Pelicula.Titulo,
                        Poster = f.Pelicula.Poster,
                        Genero = new GeneroResponse
                        {
                            Id = f.Pelicula.Genero.GeneroId,
                            Nombre = f.Pelicula.Genero.Nombre
                        }
                    },
                    Sala = new SalaResponse
                    {
                        Id = f.Sala.SalaId,
                        Nombre = f.Sala.Nombre,
                        Capacidad = f.Sala.Capacidad
                    },
                    Fecha = f.Fecha,
                    Horario = f.Horario.ToString("HH:mm")
                })
                .ToListAsync();

            if (funciones.Count() == 0)
            {
                return NotFound("No existen funciones disponibles para los criterios solicitados.");
            }

            return funciones;
        }

        // GET: api/Funciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionResponse>> GetFuncion(int id)
        {
            var funcion = await _context.Funciones.FindAsync(id);

            if (funcion == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas.FindAsync(funcion.SalaId);
            var pelicula = await _context.Peliculas.FindAsync(funcion.PeliculaId);

            var funcionResponse = new FuncionResponse
            {
                FuncionId = funcion.FuncionId,
                Sala = new SalaResponse
                {
                    Id = sala.SalaId,
                    Nombre = sala.Nombre,
                    Capacidad = sala.Capacidad
                },
                Pelicula = new PeliculaResponse
                {
                    PeliculaId = pelicula.PeliculaId,
                    Titulo = pelicula.Titulo,
                    Poster = pelicula.Poster,
                    Genero = new GeneroResponse
                    {
                        Id = pelicula.Genero.GeneroId,
                        Nombre = pelicula.Genero.Nombre
                    }
                },
                Fecha = funcion.Fecha,
                Horario = funcion.Horario.ToString("HH:mm")
            };

            return funcionResponse;
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
            int Cantidad = sala.Capacidad - cantidadTicketsPorFuncion;

            var cantidadTicketsResponse = new CantidadTicketsResponse
            {
                Cantidad = Cantidad
            };

            return cantidadTicketsResponse;
        }
        [HttpPost]
        public async Task<ActionResult<FuncionResponse>> PostFuncion(FuncionPOST funcionPOST)
        {
            // Verifica si hay funciones existentes en la misma sala que se superponen en tiempo
            DateTime nuevaFuncionInicio = funcionPOST.Horario;
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

            // Crea un objeto FuncionResponse para devolver como respuesta
            var funcionResponse = new FuncionResponse
            {
                FuncionId = funcion.FuncionId,
                Sala = new SalaResponse
                {
                    Id = funcion.Sala.SalaId,
                    Nombre = funcion.Sala.Nombre,
                    Capacidad = funcion.Sala.Capacidad
                },
                Pelicula = new PeliculaResponse
                {
                    PeliculaId = funcion.Pelicula.PeliculaId,
                    Titulo = funcion.Pelicula.Titulo,
                    Poster = funcion.Pelicula.Poster,
                    Genero = new GeneroResponse
                    {
                        Id = funcion.Pelicula.Genero.GeneroId,
                        Nombre = funcion.Pelicula.Genero.Nombre
                    }
                },
                Fecha = funcion.Fecha,
                Horario = funcion.Horario.ToString("HH:mm")
            };

            return CreatedAtAction("GetFuncion", new { id = funcion.FuncionId }, funcionResponse);
        }

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

            // En lugar de devolver NoContent(), puedes devolver un mensaje de éxito junto con la información de la función eliminada
            var funcionResponse = new FuncionResponse
            {
                FuncionId = funcion.FuncionId,
                Sala = new SalaResponse
                {
                    Id = funcion.Sala.SalaId,
                    Nombre = funcion.Sala.Nombre,
                    Capacidad = funcion.Sala.Capacidad
                },
                Pelicula = new PeliculaResponse
                {
                    PeliculaId = funcion.Pelicula.PeliculaId,
                    Titulo = funcion.Pelicula.Titulo,
                    Poster = funcion.Pelicula.Poster,
                    Genero = new GeneroResponse
                    {
                        Id = funcion.Pelicula.Genero.GeneroId,
                        Nombre = funcion.Pelicula.Genero.Nombre
                    }
                },
                Fecha = funcion.Fecha,
                Horario = funcion.Horario.ToString("HH:mm")
            };

            return Ok($"Función con ID {funcion.FuncionId} eliminada con éxito");
        }



        private bool FuncionExists(int id)
        {
            return _context.Funciones.Any(e => e.FuncionId == id);
        }
    }
}

