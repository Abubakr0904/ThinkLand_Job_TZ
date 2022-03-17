using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webapp.Entities;

namespace webapp.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Expense> Expenses { get; set; }
    
    public ApplicationDbContext(DbContextOptions options) 
        :base(options) {  }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
    }
}