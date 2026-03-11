using Core.DTO.Booking;
using Core.Models;
using Core.Models.Enums;

namespace Core.Mapper;

public static class BookingsExtensions
{
    extension(CreateBookingInfo bookingInfo)
    {
        public Booking ToBooking()
        {
            return new Booking()
            {
                RoomId = bookingInfo.RoomId,
                Tittle = bookingInfo.Tittle,
                Description = bookingInfo.Description,
                StartDate = bookingInfo.StartDate,
                EndDate = bookingInfo.EndDate,
            };
        }

        public IEnumerable<BookingParticipant> ToBookingParticipants(Guid bookingId, Guid userId)
        {
            var bookingOrganizer = new BookingParticipant()
            {
                BookingId = bookingId,
                Role = BookingRole.Organizer,
                Status = BookingParticipantStatus.Accepted,
                UserId = userId,
            };

            var bookingParticipants = bookingInfo.Participants
                .Select(p => new BookingParticipant
                {
                    BookingId = bookingId,
                    Role = BookingRole.Participant,
                    Status = BookingParticipantStatus.Pending,
                    UserId = p,
                })
                .ToList();

            bookingParticipants.Add(bookingOrganizer);
            
            return bookingParticipants;
        }
    }
}