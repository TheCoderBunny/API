using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Migrations;

public class MainDbContext : DbContext
{
    public DbSet<User> User { get; set; }
    public MainDbContext(DbContextOptions<MainDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.userId);
            entity.Property(e => e.email).IsRequired();
            entity.HasIndex(x => x.email).IsUnique();
            entity.Property(e => e.password).IsRequired();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.ticketId);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.reservationId);
        });
    }
}
