using System.Text.Json.Serialization;

namespace Core.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<BookingStatus>))]
public enum BookingStatus
{
    Active,
    Cancelled,
    Completed,
}