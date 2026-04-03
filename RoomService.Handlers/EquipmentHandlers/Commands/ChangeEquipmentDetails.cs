using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using RoomService.Core.Abstractions.Commands;
using RoomService.Domain.Exceptions;
using RoomService.Domain.ValueObjects;
using RoomService.Infrastructure.Ef;

namespace RoomService.Handlers.EquipmentHandlers.Commands;

public class ChangeEquipmentDetails
{
    public sealed record Response(ResponseDto Equipment);

    public sealed record ResponseDto(
        Guid EquipmentId,
        string Name,
        string? Description);
    
    public sealed record Command(
        EquipmentId EquipmentId,
        string? Name,
        string? Description) : ICommand<Response>;

    public sealed record ChangeDetailRequest(
        [property: MaxLength(64)] string? Name,
        [property: MaxLength(128)] string? Description) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Description))
            {
                yield return new ValidationResult("Вы ничего не меняете");
            }
        }
    }


    public sealed class Handler(RoomDbContext dbContext, ILogger<Handler> logger) : ICommandHandler<Command, Response>
    {
        public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
        {
            using var scope = logger.BeginScope("ChangeRoomDetails");
            
            var equipment = await dbContext.Equipments.FindAsync([command.EquipmentId.Value], cancellationToken);

            if (equipment is null)
            {
                throw new NotFoundException(ErrorMessages.EquipmentNotFound);
            }
            
            equipment!.ChangeDetails(command.Name, command.Description);
            
            await dbContext.SaveChangesAsync(cancellationToken);
            
            var response = new ResponseDto(equipment.Id.Value, equipment.Name, equipment.Description);
            
            return new Response(response);
        }
    }
}