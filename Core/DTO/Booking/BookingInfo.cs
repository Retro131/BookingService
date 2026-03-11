using Core.Models;
using Core.Models.Enums;

namespace Core.DTO.Booking;

public record BookingInfo(
    Guid Id,
    Guid RoomId,
    IEnumerable<ParticipantInfo> Participants,
    string Tittle,
    string? Description,
    DateTime StartDate,
    DateTime EndDate,
    BookingStatus Status);