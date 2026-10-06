using Microsoft.EntityFrameworkCore; 
using Api.Models;
namespace Api.Data;

public class AppDbContext : DbContext
{
    
   public AppDbContext(DbContextOptions<AppDbContext> option) : base(option){
   }

   public DbSet<Transaction> Transactions{
            get;
            set;
        }
    public DbSet<Category> Categories{
            get;
            set;
        }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Transaction>()
        .HasOne(t => t.Category)
        .WithMany()
        .HasForeignKey(t => t.CategoryId)
        .OnDelete(DeleteBehavior.Restrict);
}


}



