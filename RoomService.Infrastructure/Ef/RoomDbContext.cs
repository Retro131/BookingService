using Microsoft.EntityFrameworkCore;
using RoomService.Domain.Entities;
using RoomService.Infrastructure.Ef.Configuration;

namespace RoomService.Infrastructure.Ef;

public class RoomDbContext(DbContextOptions<RoomDbContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Equipment> Equipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
        modelBuilder.ApplyConfiguration(new EquipmentConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}