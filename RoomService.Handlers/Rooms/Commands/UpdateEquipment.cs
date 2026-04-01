using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using RoomService.Core.Abstractions.Commands;
using RoomService.Domain.Entities;
using RoomService.Domain.ValueObjects;
using RoomService.Infrastructure.Ef;

namespace RoomService.Handlers.Rooms.Commands;

public class UpdateEquipment
{
    public sealed record Response();

    public sealed record UpdateEquipmentRequest(
        [property: Required] ICollection<EquipmentId> EquipmentIds) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EquipmentIds.Count <= 0)
            {
                yield return new ValidationResult("Вы не передаете никакого оборудования", [nameof(EquipmentIds)]);
            }
        }
    }

    public sealed record Command(
        [property: Required] RoomId RoomId,
        [property: Required] ICollection<EquipmentId> EquipmentIds) : ICommand<Response>;

    public sealed class Handler(RoomDbContext dbContext, ILogger<Handler> logger) : ICommandHandler<Command, Response>
    {
        public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
        {
            using var scope = logger.BeginScope("ChangeRoomDetails");

            var room = dbContext.Rooms.FirstOrDefault(r => r.Id == command.RoomId);

            if (room == null)
            {
                //TODO
            }

            room!.UpdateEquipment(command.EquipmentIds);
            
            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response();
        }
    }
}