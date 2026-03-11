using BookingService.Repositories.Interfaces;
using BookingService.Services.Interfaces;
using Core.DTO.Booking;
using Core.Mapper;
using Core.Models;
using Core.Models.Enums;

namespace BookingService.Services;

public class BookingService(IBookingRepository bookingRepository) : IBookingService
{
    public async Task<Guid> CreateBookingAsync(CreateBookingInfo bookingInfo, Guid userId, CancellationToken token)
    {
        var booking = bookingInfo.ToBooking();
        var bookingId = booking.Id;

        var bookingParticipants = bookingInfo.ToBookingParticipants(bookingId, userId);

        await bookingRepository.AddBooking(booking, bookingParticipants, token);

        return bookingId;
    }

    public async Task AddParticipantsToBookingAsync(InvitedUsersToBookingInfo invitedUsersToBookingInfo,
        CancellationToken token)
    {
        var bookingId = invitedUsersToBookingInfo.BookingId;

        var bookingParticipants = invitedUsersToBookingInfo.UserIds
            .Select(userId => new BookingParticipant()
            {
                UserId = userId,
                BookingId = bookingId,
                Role = BookingRole.Participant,
                Status = BookingParticipantStatus.Pending,
            });

        await bookingRepository.AddParticipantsToBooking(bookingParticipants, token);
    }

    public async Task DeclineBookingAsync(Guid bookingId, Guid userId, CancellationToken token)
    {
        await bookingRepository.DeclineBookingAsync(bookingId, userId, token);
    }

    public async Task AcceptBookingAsync(Guid bookingId, Guid userId, CancellationToken token)
    {
        await bookingRepository.AcceptBookingAsync(bookingId, userId, token);
    }

    public async Task CancelBooking(Guid bookingId, Guid userId, CancellationToken token)
    {
        await bookingRepository.CancelBookingAsync(bookingId, userId, token);
    }

    public async Task<IEnumerable<BookingInfo>> GetBookingsByRoomId(Guid roomId, CancellationToken token)
    {
        var bookings = await bookingRepository.GetBookingsByRoomId(roomId, token);

        return bookings;
    }

    public Task<UserBookingInfo> GetBookingsByUserId(Guid userId, CancellationToken token)
    {
        return Task.FromResult(new UserBookingInfo(userId, []));
    }
}