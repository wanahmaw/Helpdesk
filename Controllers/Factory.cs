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
    public static class Factory
    {
        public static TicketInfo GetTicketFromId(int ticketId, HelpDeskContext _context)
        {
            // Declare database entities
            var tickets = _context.Ticket;
            var userTickets = _context.UserTickets;
            var users = _context.User;

            // Query for a ticket Info response
            var res =
                from u in userTickets
                where u.TicketId == ticketId
                select new TicketInfo()
                {
                    TicketId = u.TicketId,
                    OwnerId = u.User.Id,
                    Title = u.Ticket.Title,
                    Content = u.Ticket.Content,
                    OwnerName = u.User.UserName,
                };

            return res.FirstOrDefault();
        }

        public static IEnumerable<TicketInfo> GetTicketsFromUser(int userId, HelpDeskContext _context)
        {
            // Declare database entities
            var tickets = _context.Ticket;
            var userTickets = _context.UserTickets;
            var users = _context.User;

            // Query for tickets from user
            var res =
                from u in userTickets
                where u.UserId == userId
                select new TicketInfo()
                {
                    TicketId = u.TicketId,
                    OwnerId = u.User.Id,
                    Title = u.Ticket.Title,
                    Content = u.Ticket.Content,
                    OwnerName = u.User.UserName,
                };

            return res.ToList();
        }

        public static async Task<Ticket> CreateTicket(TicketForm ticketForm, HelpDeskContext _context)
        {
            // Create ticket
            Ticket ticket = new Ticket()
            {
                Title = ticketForm.Title,
                Content = ticketForm.Content
            };

            // Add ticket to database
            _context.Ticket.Add(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        public static void CreateTicketUserAssociation(int userId, int ticketId, HelpDeskContext _context)
        {
            // Create association with user
            UserTickets userTicket = new UserTickets()
            {
                UserId = userId,
                TicketId = ticketId
            };

            // Add association to database
            _context.UserTickets.Add(userTicket);
            _context.SaveChanges();
        }
    }
}