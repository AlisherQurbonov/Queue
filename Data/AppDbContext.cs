using Microsoft.EntityFrameworkCore;
using queue.Entities;

namespace queue.Data;

public class AppDbContext : DbContext
{
    public DbSet<Register> Registers { get; set; }
    
     public AppDbContext(DbContextOptions options)
        : base(options) { }
}