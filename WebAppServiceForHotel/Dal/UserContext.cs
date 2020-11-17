using Microsoft.EntityFrameworkCore;
using System;

namespace WebAppServiceForHotel.DAL
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

    }
}
