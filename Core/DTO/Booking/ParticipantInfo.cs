using Core.Models.Enums;

namespace Core.DTO.Booking;

public record ParticipantInfo(Guid ParticipantId, BookingRole Role, BookingParticipantStatus Status);