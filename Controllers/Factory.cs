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
        public static TicketOnly CreateTicketFromId(int id, DbSet<Ticket> tickets, DbSet<UserTickets> userTickets, DbSet<User> users)
        {
            var res =
                from u in userTickets
                where u.TicketId == id
                select new TicketOnly()
                {
                    TicketId = u.TicketId,
                    OwnerId = u.User.Id,
                    Title = u.Ticket.Title,
                    Content = u.Ticket.Content,
                    OwnerName = u.User.UserName,
                };

            return res.FirstOrDefault();
        }
    }
}