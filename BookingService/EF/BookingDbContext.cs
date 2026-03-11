using BookingService.EF.Configuration;
using Core.Models;
using Core.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookingService.EF;

public class BookingDbContext(DbContextOptions<BookingDbContext> options) : DbContext(options)
{
    public DbSet<Booking> Bookings { get; set; } 
    public DbSet<BookingParticipant> BookingParticipants { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
        modelBuilder.ApplyConfiguration(new BookingParticipantConfiguration());
        
        modelBuilder.HasPostgresEnum<BookingStatus>( name: "booking_status");
        modelBuilder.HasPostgresEnum<BookingRole>( name: "booking_role");
        modelBuilder.HasPostgresEnum<BookingParticipantStatus>( name: "booking_participant_status");
    }
}