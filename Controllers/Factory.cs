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
            var userTickets = _context.UserTickets;

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

        public static async Task<User> CreateUser(Login login, string hashedPw, HelpDeskContext _context)
        {
            // Create new user object
            User newUser = new User()
            {
                UserName = login.UserName,
                UserPassword = hashedPw
            };

            // Add user to database
            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public static async Task<UserRoles> CreateUserRoleAssociation(int newUserId, int roleId, HelpDeskContext _context)
        {
            UserRoles newUserRole = new UserRoles()
            {
                UserId = newUserId,
                RoleId = roleId
            };

            // Add user role to database
            _context.UserRoles.Add(newUserRole);
            await _context.SaveChangesAsync();

            return newUserRole;
        }

        public static Identity CreateIdentity(int userId, HelpDeskContext _context)
        {

            var userRoles = _context.UserRoles;

            // Query for identity
            var res =
                from u in userRoles
                where u.UserId == userId
                select new Identity()
                {
                    UserName = u.User.UserName,
                    RoleName = u.Role.Title,
                    UserId = userId,
                    RoleId = u.RoleId
                };

            return res.FirstOrDefault();
        }
    }
}