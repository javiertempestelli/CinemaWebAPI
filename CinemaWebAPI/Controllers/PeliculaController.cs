using CinemaWebAPI.Application.DTOs.Pelicula;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculaController : ControllerBase
    {
        private readonly CinemaContext _context;

        public PeliculaController(CinemaContext context)
        {
            _context = context;
        }

        // GET: api/Peliculas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PeliculaResponse>> GetPelicula(int id)
        {
            var pelicula = await _context.Peliculas
                .Include(p => p.Genero)
                .Where(p => p.PeliculaId == id)
                .Select(p => new PeliculaResponse
                {
                    PeliculaId = p.PeliculaId,
                    Titulo = p.Titulo,
                    Poster = p.Poster,
                    Trailer = p.Trailer,
                    Sonopsis = p.Sonopsis,
                    Genero = new GeneroResponse
                    {
                        Id = p.Genero.GeneroId,
                        Nombre = p.Genero.Nombre
                    },
                    Funciones = _context.Funciones
                        .Where(f => f.PeliculaId == id)
                        .Select(f => new FuncionResponse
                        {
                            FuncionId = f.FuncionId,
                            Fecha = f.Fecha,
                            Horario = f.Horario.ToString("HH:mm")
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (pelicula == null)
            {
                return NotFound();
            }

            return pelicula;
        }

        // PUT: api/Peliculas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPelicula(int id, PeliculaPUT peliculaPUT)
        {
            if (id != peliculaPUT.PeliculaId)
            {
                return BadRequest();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);

            if (pelicula == null)
            {
                return NotFound();
            }

            // Actualiza los campos de la entidad Pelicula con los valores de peliculaPUT.
            pelicula.Titulo = peliculaPUT.Titulo;
            pelicula.Poster = peliculaPUT.Poster;
            pelicula.Trailer = peliculaPUT.Trailer;
            pelicula.Sonopsis = peliculaPUT.Sonopsis;
            pelicula.GeneroId = peliculaPUT.Genero;

            _context.Entry(pelicula).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeliculaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Devuelve la película actualizada
            var peliculaResponse = new PeliculaResponse
            {
                PeliculaId = pelicula.PeliculaId,
                Titulo = pelicula.Titulo,
                Poster = pelicula.Poster,
                Trailer = pelicula.Trailer,
                Sonopsis = pelicula.Sonopsis,
                //GeneroId = pelicula.GeneroId
            };

            return Ok(peliculaResponse);
        }

        private bool PeliculaExists(int id)
        {
            return _context.Peliculas.Any(e => e.PeliculaId == id);
        }

    }
}
