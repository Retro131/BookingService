namespace Core.DTO.Booking;

public record UserBookingInfo(
    Guid UserId,
    IEnumerable<BookingInfo> Bookings);