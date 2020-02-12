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
    public class UserTicketsController : ControllerBase
    {
        private readonly HelpDeskContext _context;

        public UserTicketsController(HelpDeskContext context)
        {
            _context = context;
        }

        // GET: api/UserTickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTickets>>> GetUserTickets()
        {
            return await _context.UserTickets.ToListAsync();
        }

        // GET: api/UserTickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTickets>> GetUserTickets(int id)
        {
            var userTickets = await _context.UserTickets.FindAsync(id);

            if (userTickets == null)
            {
                return NotFound();
            }

            return userTickets;
        }

        // PUT: api/UserTickets/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTickets(int id, UserTickets userTickets)
        {
            if (id != userTickets.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userTickets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTicketsExists(id))
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

        // POST: api/UserTickets
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<UserTickets>> PostUserTickets(UserTickets userTickets)
        {
            _context.UserTickets.Add(userTickets);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserTicketsExists(userTickets.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserTickets", new { id = userTickets.UserId }, userTickets);
        }

        // DELETE: api/UserTickets/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserTickets>> DeleteUserTicketAssociation(int id)
        {
            var userTickets = await _context.UserTickets.FirstOrDefaultAsync(t => t.TicketId == id);
            if (userTickets == null)
            {
                return NotFound();
            }

            _context.UserTickets.Remove(userTickets);
            await _context.SaveChangesAsync();

            return userTickets;
        }

        private bool UserTicketsExists(int id)
        {
            return _context.UserTickets.Any(e => e.UserId == id);
        }
    }
}
