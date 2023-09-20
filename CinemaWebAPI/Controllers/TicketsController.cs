using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            if (_context.Tickets == null)
            {
                return NotFound();
            }
            return await _context.Tickets.ToListAsync();
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(Guid id)
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

            return ticket;
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<ActionResult<TicketDTO>> PostTicket(TicketDTO ticketDTO)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'CinemaContext.Tickets' is null.");
            }

            // Aquí puedes agregar lógica para verificar la disponibilidad de boletos, validar datos, etc.

            var ticket = new Ticket
            {
                FuncionId = ticketDTO.FuncionId,
                Usuario = ticketDTO.Usuario
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            // Puedes mapear el ticket a un TicketDTO si es necesario

            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticketDTO);
        }

        // POST: api/Tickets/{id}/tickets
        [HttpPost("{id}/tickets")]
        public async Task<ActionResult<TicketDTO>> ComprarTickets(int id, TicketDTO ticketDTO)
        {
            // Verifica si la función existe
            var funcion = await _context.Funciones.FindAsync(id);

            if (funcion == null)
            {
                return NotFound("Función no encontrada.");
            }

            // Aquí puedes agregar lógica adicional para verificar la disponibilidad de boletos, validar datos, etc.

            var ticket = new Ticket
            {
                FuncionId = id,
                Usuario = ticketDTO.Usuario
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            // Puedes mapear el ticket a un TicketDTO si es necesario

            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticketDTO);
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

        private bool TicketExists(Guid id)
        {
            return (_context.Tickets?.Any(e => e.TicketId == id)).GetValueOrDefault();
        }
    }
}
