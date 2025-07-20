using Microsoft.EntityFrameworkCore;
using People.Data.Entities;

namespace People.Data.Context
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
             
            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, Name = "Alice Smith", DateOfBirth = new DateOnly(1990, 5, 15) },
                new Person { Id = 2, Name = "Bob Johnson", DateOfBirth = new DateOnly(1985, 10, 20) },
                new Person { Id = 3, Name = "Charlie Brown", DateOfBirth = new DateOnly(1992, 3, 8) }
            );
        }
    }
}
