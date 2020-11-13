using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
