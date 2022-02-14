using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using queue.Entities;

namespace queue.Data;

public class AppDbContext : IdentityDbContext<User>
{
  public DbSet<Register> Registers { get; set; }

  public AppDbContext (DbContextOptions<AppDbContext> options) : base (options) { }
  
}