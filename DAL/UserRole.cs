using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class UserRole
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public User Users { get; set; }
        public Role Roles { get; set; }
    }
}
