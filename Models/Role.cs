using System;
using System.Collections.Generic;

namespace Helpdesk.Models
{
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }

    public static class RoleTitle
    {
        public const string Team = "Team";
        public const string User = "User";
    }
}
