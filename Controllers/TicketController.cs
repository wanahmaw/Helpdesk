using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Helpdesk.Models;

namespace Helpdesk.Controllers
{

    [Authorize]
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
        [Authorize(Roles = "team")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketInfo>>> GetTicket()
        {
            var tickets = await Factory.GetAllTickets(_context);
            return tickets.ToList();
        }

        // GET: api/Ticket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketInfo>> GetTicket(int id)
        {
            // Verify ticket exists
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound("Ticket not found");
            }

            // Create response for front end
            var ticketFromId = Factory.GetTicketFromId(id, _context);

            // Verify user can only retrieve their tickets
            var currentUserId = int.Parse(User.Identity.Name);
            if (ticketFromId.OwnerId != currentUserId && !User.IsInRole("team"))
            {
                return Forbid();
            }

            return ticketFromId;
        }

        // GET: api/Ticket/User/5
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<TicketInfo>>> GetTicketUser(int id)
        {
            // Only allow team to access other user tickets
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole("team"))
            {
                return Forbid();
            }

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

            // Create ticket
            var ticket = await Factory.CreateTicket(ticketForm, _context);

            // Create ticket & user association
            if (ticket.Id > 0)
            {
                Factory.CreateTicketUserAssociation(userId, ticket.Id, _context);
            }

            return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
        }

        // DELETE: api/Ticket/5
        [Authorize(Roles = "team")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ticket>> DeleteTicket(int id)
        {
            // Verify ticket
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            // Remove associated user relation
            var controller = new UserTicketsController(_context);
            var relation = await controller.DeleteUserTicketAssociation(id);

            // Remove ticket
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
