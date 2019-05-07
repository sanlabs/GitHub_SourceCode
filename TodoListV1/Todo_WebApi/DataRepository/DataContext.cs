using Microsoft.EntityFrameworkCore;
using System;

namespace DataRepository
{
    public class TodoDBContext : DbContext
    {
        public TodoDBContext(DbContextOptions<TodoDBContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Event> Events { get; set; }

        public DbSet<Models.User> Users { get; set; }
    }
}
