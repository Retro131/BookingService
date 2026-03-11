using System.ComponentModel.DataAnnotations;
using Core.Models.Enums;

namespace Core.Models;

public class Booking
{
    public Guid Id { get; } = Guid.CreateVersion7();
    
    public Guid RoomId { get; init; }
    
    [MaxLength(256)]
    public required string Tittle { get; init; }
    
    [MaxLength(1024)]
    public string? Description { get; init; }
    
    public DateTime StartDate { get; init; }
    
    public DateTime EndDate { get; init; }

    public BookingStatus Status { get; set; } = BookingStatus.Active;
    
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    public ICollection<BookingParticipant> Participants { get; set; } = [];
}