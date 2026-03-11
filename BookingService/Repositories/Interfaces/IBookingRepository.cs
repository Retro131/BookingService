using Core.DTO.Booking;
using Core.Models;

namespace BookingService.Repositories.Interfaces;

public interface IBookingRepository
{
    Task AddBooking(Booking bookingInfo, IEnumerable<BookingParticipant> userId, CancellationToken token);
    
    Task AddParticipantsToBooking(IEnumerable<BookingParticipant> invitedUsersToBooking, CancellationToken token);
    Task<IEnumerable<BookingInfo>> GetBookingsByRoomId(Guid roomId, CancellationToken token);
    Task DeclineBookingAsync(Guid bookingId, Guid userId, CancellationToken token);
    Task AcceptBookingAsync(Guid bookingId, Guid userId, CancellationToken token);
    Task CancelBookingAsync(Guid bookingId, Guid userId, CancellationToken token);
}