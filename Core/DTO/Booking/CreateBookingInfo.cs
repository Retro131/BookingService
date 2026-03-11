using System.ComponentModel.DataAnnotations;

namespace Core.DTO.Booking;

public record CreateBookingInfo(
    Guid RoomId,
    string Tittle,
    string? Description,
    IEnumerable<Guid> Participants,
    DateTime StartDate,
    DateTime EndDate
    ) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate < DateTime.UtcNow)
            yield return new ValidationResult(
                "Время начала не может быть раньше текущего времени",
                [nameof(StartDate)]
            );
        
        if (EndDate < StartDate)
            yield return new ValidationResult(
                "Конец должен быть после начала",
                [nameof(EndDate)]
            );
        
        if (Description is not null && Description.Length > 1024)
            yield return new ValidationResult(
                "Описание слишком длинное",
                [nameof(Description)]);
        
        
    }
}