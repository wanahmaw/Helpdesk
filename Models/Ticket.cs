using System;
using System.Collections.Generic;

namespace Helpdesk.Models
{
    public partial class Ticket
    {
        public Ticket()
        {
            UserTickets = new HashSet<UserTickets>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public virtual ICollection<UserTickets> UserTickets { get; set; }
    }

    public partial class TicketInfo
    {
        public int TicketId { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public partial class TicketForm
    {
        public int OwnerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
