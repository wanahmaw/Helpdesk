using System;
using System.Collections.Generic;

namespace Helpdesk.Models
{
    public partial class Identity
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Token { get; set; }
    }
}