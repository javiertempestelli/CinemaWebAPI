using Application.DTOs.Pelicula;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaWebAPI.Application.DTOs.Pelicula;

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

        // GET: api/Peliculas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PeliculaGET>>> GetPeliculas()
        {
            var peliculas = await _context.Peliculas
                .Join(
                    _context.Generos,
                    p => p.GeneroId,
                    g => g.GeneroId,
                    (p, g) => new PeliculaGET
                    {
                        Titulo = p.Titulo,
                        Poster = p.Poster,
                        Trailer = p.Trailer,
                        Sonopsis = p.Sonopsis,
                        GeneroNombre = g.Nombre
                    }
                )
                .ToListAsync();

            return peliculas;
        }


        // GET: api/Peliculas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PeliculaGET>> GetPelicula(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            
            if (pelicula == null)
            {
                return NotFound();
            }
            var genero = await _context.Generos.FindAsync(pelicula.GeneroId);
            var peliculaDTO = await _context.Peliculas
                .Where(p => p.PeliculaId == id)
                .Select(p => new PeliculaGET
                {
                    Titulo = p.Titulo,
                    Poster = p.Poster,
                    Trailer = p.Trailer,
                    Sonopsis = p.Sonopsis,
                    GeneroNombre = genero.Nombre

                    // Mapear otros campos DTO aquí
                })
                .FirstOrDefaultAsync();



            return peliculaDTO;
        }

        // PUT: api/Peliculas/5/Titulo
        [HttpPut("{id}/Titulo")]
        public async Task<IActionResult> PutPeliculaTitulo(int id, [FromBody] string nuevoTitulo)
        {
            if (_context.Peliculas == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);

            if (pelicula == null)
            {
                return NotFound();
            }

            // Verificar si ya existe una película con el nuevo título
            var existeOtraPeliculaConMismoTitulo = await _context.Peliculas.AnyAsync(p => p.Titulo == nuevoTitulo);

            if (existeOtraPeliculaConMismoTitulo)
            {
                return BadRequest("Ya existe una película con el mismo título.");
            }

            pelicula.Titulo = nuevoTitulo;

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

            return NoContent();
        }


        // PUT: api/Peliculas/5/Poster
        [HttpPut("{id}/Poster")]
        public async Task<IActionResult> PutPeliculaPoster(int id, [FromBody] string nuevoPoster)
        {
            if (_context.Peliculas == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);

            if (pelicula == null)
            {
                return NotFound();
            }

            pelicula.Poster = nuevoPoster;

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

            return NoContent();
        }

        // PUT: api/Peliculas/5/Trailer
        [HttpPut("{id}/Trailer")]
        public async Task<IActionResult> PutPeliculaTrailer(int id, [FromBody] string nuevoTrailer)
        {
            if (_context.Peliculas == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);

            if (pelicula == null)
            {
                return NotFound();
            }

            pelicula.Trailer = nuevoTrailer;

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

            return NoContent();
        }
        
        // PUT: api/Peliculas/5/Titulo
        [HttpPut("{id}/Sonopsis")]
        public async Task<IActionResult> PutPeliculaSonopsis(int id, [FromBody] string nuevaSonopsis)
        {
            if (_context.Peliculas == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);

            if (pelicula == null)
            {
                return NotFound();
            }

            pelicula.Sonopsis = nuevaSonopsis;

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

            return NoContent();
        }
        // POST: api/Peliculas
        [HttpPost]
        public async Task<ActionResult<PeliculaPOST>> PostPelicula(PeliculaPOST peliculaPOST)
        {
            // Verificar si ya existe una película con el mismo título
            bool peliculaExists = await _context.Peliculas.AnyAsync(p => p.Titulo == peliculaPOST.Titulo);

            if (peliculaExists)
            {
                return BadRequest("Ya existe una película con el mismo título.");
            }

            var pelicula = new Pelicula
            {
                Titulo = peliculaPOST.Titulo,
                Poster = peliculaPOST.Poster,
                Trailer = peliculaPOST.Trailer,
                Sonopsis = peliculaPOST.Sonopsis,
                GeneroId = peliculaPOST.GeneroId
            };

            _context.Peliculas.Add(pelicula);
            await _context.SaveChangesAsync();

            // Crea un nuevo DTO con el ID generado y otros campos
            var newPeliculaPOST = new PeliculaPOST
            {
                Titulo = pelicula.Titulo,
                Poster = pelicula.Poster,
                Trailer = pelicula.Trailer,
                Sonopsis = pelicula.Sonopsis,
                GeneroId = pelicula.GeneroId
            };

            return CreatedAtAction("GetPelicula", new { id = newPeliculaPOST.PeliculaId }, newPeliculaPOST);
        }

        // DELETE: api/Peliculas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePelicula(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }

            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PeliculaExists(int id)
        {
            return _context.Peliculas.Any(e => e.PeliculaId == id);
        }
    }
}
