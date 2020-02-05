using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Helpdesk.Models;

namespace Helpdesk.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly HelpDeskContext _context;

        public TicketController(HelpDeskContext context)
        {
            _context = context;
        }

        // GET: api/Ticket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicket()
        {
            return await _context.Ticket.ToListAsync();
        }

        // GET: api/Ticket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketOnly>> GetTicket(int id)
        {
            // Declare DB variables
            var tickets = _context.Ticket;
            var userTickets = _context.UserTickets;
            var users = _context.User;

            // Verify ticket exists
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
                return NotFound();

            // Create response for front end
            var response = Factory.CreateTicketFromId(id, tickets, userTickets, users);

            return response;
        }

        // PUT: api/Ticket/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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

        // POST: api/Ticket
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            _context.Ticket.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
        }

        // DELETE: api/Ticket/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ticket>> DeleteTicket(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }
}
