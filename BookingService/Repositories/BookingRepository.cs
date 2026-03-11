using BookingService.EF;
using BookingService.Repositories.Interfaces;
using Core.DTO.Booking;
using Core.Models;
using Core.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories;

public class BookingRepository(BookingDbContext dbContext) : IBookingRepository
{
    public async Task AddBooking(Booking booking, IEnumerable<BookingParticipant> bookingParticipants,
        CancellationToken token)
    {
        dbContext.Bookings.Add(booking);

        dbContext.BookingParticipants.AddRange(bookingParticipants);

        await dbContext.SaveChangesAsync(token);
    }

    public async Task AddParticipantsToBooking(IEnumerable<BookingParticipant> invitedUsersToBooking,
        CancellationToken token)
    {
        dbContext.BookingParticipants.AddRange(invitedUsersToBooking);

        await dbContext.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<BookingInfo>> GetBookingsByRoomId(Guid roomId, CancellationToken token)
    {
        var bookings = await dbContext.Bookings
            .Where(b => b.RoomId == roomId)
            .Select(b => new BookingInfo(
                b.Id,
                b.RoomId,
                Participants: b.Participants.Select(p => new ParticipantInfo(p.UserId, p.Role, p.Status)),
                b.Tittle,
                b.Description,
                b.EndDate,
                b.StartDate,
                b.Status))
            .ToListAsync(token);

        return bookings;
    }

    public async Task DeclineBookingAsync(Guid bookingId, Guid userId, CancellationToken token)
    {
        await dbContext.BookingParticipants
            .Where(b => b.UserId == userId && b.BookingId == bookingId)
            .ExecuteUpdateAsync(s => 
                s.SetProperty(b => 
                    b.Status, BookingParticipantStatus.Declined), token);
    }

    public async Task AcceptBookingAsync(Guid bookingId, Guid userId, CancellationToken token)
    {
        await dbContext.BookingParticipants
            .Where(b => b.UserId == userId && b.BookingId == bookingId)
            .ExecuteUpdateAsync(s => 
                s.SetProperty(b => 
                    b.Status, BookingParticipantStatus.Accepted), token);
    }

    public async Task CancelBookingAsync(Guid bookingId, Guid userId, CancellationToken token)
    {
        await dbContext.Bookings
            .Where(b => b.Id == bookingId)
            .Where(b =>
                b.Participants.Any(p => p.UserId == userId && p.Role == BookingRole.Organizer))
            .ExecuteUpdateAsync(s => 
                s.SetProperty(b => 
                    b.Status, BookingStatus.Cancelled), token);
    }
}