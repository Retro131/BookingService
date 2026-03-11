namespace Core.DTO.Booking;

public record InvitedUsersToBookingInfo(
    Guid BookingId,
    IEnumerable<Guid> UserIds);