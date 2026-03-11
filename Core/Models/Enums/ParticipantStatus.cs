using System.Text.Json.Serialization;

namespace Core.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<BookingParticipantStatus>))]
public enum BookingParticipantStatus
{
    Accepted,
    Declined,
    Pending
}