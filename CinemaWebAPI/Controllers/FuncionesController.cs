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
        public async Task<ActionResult<FuncionDTO>> PostFuncion(FuncionPOST funcionPOST)
        {
            
            var funcion = new Funcion
            {
                SalaId = funcionPOST.SalaId,
                Fecha = funcionPOST.Fecha,
                Horario = funcionPOST.Horario,
                PeliculaId = funcionPOST.PeliculaId
            };

            _context.Funciones.Add(funcion);
            await _context.SaveChangesAsync();

            // Resto del código para crear la respuesta...

            return CreatedAtAction("GetFuncion", new { id = funcion.FuncionId }, funcionPOST);
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


        private bool FuncionExists(int id)
        {
            return _context.Funciones.Any(e => e.FuncionId == id);
        }
    }
}

