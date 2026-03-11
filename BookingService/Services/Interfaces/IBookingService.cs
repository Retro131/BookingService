using Core.DTO.Booking;

namespace BookingService.Services.Interfaces;

public interface IBookingService
{
    Task<Guid> CreateBookingAsync(CreateBookingInfo bookingInfo, Guid userId, CancellationToken token);
    Task<IEnumerable<BookingInfo>> GetBookingsByRoomId(Guid roomId, CancellationToken token);
    Task<UserBookingInfo> GetBookingsByUserId(Guid userId, CancellationToken token);
    Task AddParticipantsToBookingAsync(InvitedUsersToBookingInfo invitedUsersToBookingInfo, CancellationToken token);
    Task DeclineBookingAsync(Guid bookingId, Guid userId, CancellationToken token);
    Task AcceptBookingAsync(Guid bookingId, Guid userId, CancellationToken token);
    Task CancelBooking(Guid bookingId, Guid userId, CancellationToken token);
}