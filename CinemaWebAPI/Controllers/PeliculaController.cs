﻿using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCinema;

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
        public async Task<ActionResult<IEnumerable<PeliculaDTO>>> GetPeliculas()
        {
            var peliculas = await _context.Peliculas
                .Select(p => new PeliculaDTO
                {
//                    PeliculaId = p.PeliculaId,
                    Titulo = p.Titulo,
                    Poster = p.Poster,
                    Trailer = p.Trailer,
                    Sonopsis = p.Sonopsis
                })
                .ToListAsync();

            return peliculas;
        }

        // GET: api/Peliculas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PeliculaDTO>> GetPelicula(int id)
        {
            var pelicula = await _context.Peliculas
                .Where(p => p.PeliculaId == id)
                .Select(p => new PeliculaDTO
                {
//                    PeliculaId = p.PeliculaId,
                    Titulo = p.Titulo,
                    Poster = p.Poster,
                    Trailer = p.Trailer,
                    Sonopsis = p.Sonopsis
                    // Mapear otros campos DTO aquí
                })
                .FirstOrDefaultAsync();

            if (pelicula == null)
            {
                return NotFound();
            }

            return pelicula;
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
        public async Task<ActionResult<PeliculaDTO>> PostPelicula(PeliculaDTO peliculaDTO)
        {
            var pelicula = new Pelicula
            {
                Titulo = peliculaDTO.Titulo,
                // Asigna otros valores desde el DTO aquí
            };

            _context.Peliculas.Add(pelicula);
            await _context.SaveChangesAsync();

            // Crea un nuevo DTO con el ID generado y otros campos
            var newPeliculaDTO = new PeliculaDTO
            {
                Titulo = pelicula.Titulo,
                Poster = pelicula.Poster,
                Trailer = pelicula.Trailer,
                Sonopsis = pelicula.Sonopsis

                // Mapea otros campos DTO aquí
            };

            return CreatedAtAction("GetPelicula", new { id = newPeliculaDTO.PeliculaId }, newPeliculaDTO);
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
