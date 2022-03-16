using Microsoft.EntityFrameworkCore;
using webapp.Entities;

namespace webapp.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Expense> Expenses { get; set; }
    
    public ApplicationDbContext(DbContextOptions options) 
        :base(options) {  }
}