using BulkyWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Data
{
    //DbContext used Entity Framework COre
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
                
        }

        //this will create a table called Categories based on Category Model
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                 new Category { Id = 2, Name = "Sci-fi", DisplayOrder = 2 },
                  new Category { Id = 3, Name = "Comedy", DisplayOrder = 3 });
        }

    }
}
