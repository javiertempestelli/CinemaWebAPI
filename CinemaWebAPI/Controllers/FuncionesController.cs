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
                .Include(f => f.Sala) // Cargar la relación con la tabla Sala
                .Include(f => f.Pelicula) // Cargar la relación con la tabla Película
                .Select(f => new FuncionDTO
                {
                    SalaNombre = f.Sala.Nombre,
                    Fecha = f.Fecha,
                    Horario = f.Horario,
                    PeliculaTitulo = f.Pelicula.Titulo
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
            var sala = await _context.Salas.FindAsync(funcion.SalaId); // Obtén la sala correspondiente
            var pelicula = await _context.Peliculas.FindAsync(funcion.PeliculaId);

            var funcionDTO = new FuncionDTO
            {
                SalaId = funcion.SalaId,
                SalaNombre = sala != null ? sala.Nombre : "No existe la sala",
                Fecha = funcion.Fecha,
                Horario = funcion.Horario,
                PeliculaId = funcion.PeliculaId,
                PeliculaTitulo = pelicula != null ? pelicula.Titulo : "No existe la pelicula"
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
            var sala = await _context.Salas.FindAsync(funcion.SalaId); // Obtén la sala correspondiente
            int cantidadTickets = await _context.Tickets.CountAsync(t => t.FuncionId == id);
            int cantidadDisponible = sala.Capacidad - cantidadTickets;

            var cantidadTicketsResponse = new CantidadTicketsResponse
            {
                Cantidad = cantidadDisponible
            };

            return cantidadTicketsResponse;
        }

        [HttpPost]
        public async Task<ActionResult<FuncionDTO>> PostFuncion(FuncionDTO funcionDTO)
        {
            var funcion = new Funcion
            {
                SalaId = funcionDTO.SalaId,
                Fecha = funcionDTO.Fecha,
                Horario = funcionDTO.Horario,
                PeliculaId = funcionDTO.PeliculaId
            };

            _context.Funciones.Add(funcion);
            await _context.SaveChangesAsync();

            // Resto del código para crear la respuesta...

            return CreatedAtAction("GetFuncion", new { id = funcion.FuncionId }, funcionDTO);
        }


        //[HttpPost]
        //public async Task<ActionResult<FuncionDTO>> Post2Funcion(FuncionDTO funcionDTO)
        //{
        //    if (funcionDTO == null)
        //    {
        //        return BadRequest("El objeto funcionDTO es nulo.");
        //    }

        //    // Mapea los campos de funcionDTO a la entidad Funcion.
        //    var funcion = new Funcion
        //    {
        //        SalaId = funcionDTO.SalaId,
        //        Fecha = DateTime.Parse(funcionDTO.Fecha.ToString("yyyy-MM-dd")),
        //        Horario = DateTime.Parse(funcionDTO.Horario.ToString("HH:mm:ss")),
        //        PeliculaId = funcionDTO.PeliculaId
        //    };

        //    _context.Funciones.Add(funcion);
        //    await _context.SaveChangesAsync();


        //    return CreatedAtAction("GetFuncion", new { id = funcion.FuncionId }, funcionDTO);
        //}

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


        private bool FuncionExists(int id)
        {
            return _context.Funciones.Any(e => e.FuncionId == id);
        }
    }
}

