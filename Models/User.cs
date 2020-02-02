using System;
using System.Collections.Generic;

namespace Helpdesk.Models
{
    public partial class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRoles>();
            UserTickets = new HashSet<UserTickets>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
        public virtual ICollection<UserTickets> UserTickets { get; set; }
    }
}
