using System;
using System.Collections.Generic;

namespace Helpdesk.Models
{
    public partial class UserTickets
    {
        public int UserId { get; set; }
        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual User User { get; set; }
    }
}
