using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Ticket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCinema;

namespace CinemaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly CinemaContext _context;

        public TicketsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketGETALL>>> GetTickets()
        {
            var tickets = await _context.Tickets
                .Include(ticket => ticket.Funcion) // Incluye la relación con Funcion
                .ThenInclude(funcion => funcion.Pelicula) // Incluye la relación con Pelicula dentro de Funcion
                .Select(ticket => new TicketGETALL
                {
                    TicketId = ticket.TicketId,
                    FuncionId = ticket.FuncionId,
                    Usuario = ticket.Usuario,
                    PeliculaTitulo = ticket.Funcion.Pelicula.Titulo
                })
                .ToListAsync();

            return tickets;
        }



        // GET: api/Tickets/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Ticket>> GetTicket(Guid id)
        //{
        //    if (_context.Tickets == null)
        //    {
        //        return NotFound();
        //    }
        //    var ticket = await _context.Tickets.FindAsync(id);

        //    if (ticket == null)
        //    {
        //        return NotFound();
        //    }

        //    return ticket;
        //}


        // POST: api/Tickets/{id}/tickets
        [HttpPost("{id}/tickets")]
        public async Task<ActionResult<TicketPOST>> ComprarTickets(int id, TicketPOST ticketDTO)
        {
            // Verifica si la función existe
            var funcion = await _context.Funciones.FindAsync(id);

            if (funcion == null)
            {
                return NotFound("Función no encontrada.");
            }
            var sala = await _context.Salas.FindAsync(funcion.SalaId);
            // Verifica cuántos boletos se han vendido para esta función
            int boletosVendidos = await _context.Tickets.CountAsync(t => t.FuncionId == id);

            // Verifica si no hay boletos disponibles (todos los boletos han sido vendidos)
            if (boletosVendidos >= sala.Capacidad)
            {
                return BadRequest("No hay boletos disponibles para esta función.");
            }

            var ticket = new Ticket
            {
                Usuario = ticketDTO.Usuario
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            // Puedes mapear el ticket a un TicketDTO si es necesario
            var ticketResponse = new TicketPOST
            {
                TicketId = ticket.TicketId,
                FuncionId = ticket.FuncionId,
                Usuario = ticket.Usuario
                // Agregar otros campos si es necesario
            };

            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticketResponse);
        }



        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            if (_context.Tickets == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
