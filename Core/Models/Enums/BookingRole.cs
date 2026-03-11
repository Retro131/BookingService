using System.Text.Json.Serialization;

namespace Core.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<BookingRole>))]
public enum BookingRole
{
    Organizer,
    Participant,
}