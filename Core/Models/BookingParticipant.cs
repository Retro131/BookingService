using Core.Models.Enums;

namespace Core.Models;

public class BookingParticipant
{
    public Guid UserId { get; init; }
    
    public Guid BookingId { get; init; }
    
    public BookingParticipantStatus Status { get; set; }
    
    public BookingRole Role { get; set; }

    public Booking Booking { get; set; } = null!;
}