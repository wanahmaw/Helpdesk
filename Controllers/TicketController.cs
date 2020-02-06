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
        public async Task<ActionResult<TicketInfo>> GetTicket(int id)
        {
            // Verify ticket exists
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound("User not found");
            }

            // Create response for front end
            var ticketFromId = Factory.GetTicketFromId(id, _context);

            return ticketFromId;
        }

        // GET: api/Ticket/User/5
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<TicketInfo>>> GetTicketUser(int id)
        {
            // Verify user exists
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Create response for front end
            var ticketsFromUser = Factory.GetTicketsFromUser(id, _context);

            return ticketsFromUser.ToList();
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
        public async Task<ActionResult<Ticket>> PostTicket(TicketForm ticketForm)
        {
            // Verify user
            int userId = ticketForm.OwnerId;
            bool userExists = _context.User.Any(e => e.Id == userId);
            if (userExists == false)
            {
                return NotFound("User not found");
            }

            // Factory to create ticket
            var ticket = await Factory.CreateTicket(ticketForm, _context);

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
