using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using RoomService.Core.Abstractions.Commands;
using RoomService.Domain.Entities;
using RoomService.Domain.ValueObjects;
using RoomService.Infrastructure.Ef;

namespace RoomService.Handlers.Rooms.Commands;

public class CreateRoom
{
    public sealed record Response(Guid Id);

    public sealed record Command(
        string Name,
        Address Location,
        int Capacity,
        string? Description,
        ICollection<Equipment> Equipments) : ICommand<Response>;

    public sealed record CreateRoomRequest(
        [property: MaxLength(64)] string Name,
        [property: Required] Address Location,
        [property: Required] int Capacity,
        [property: MaxLength(128)] string? Description,
        [property: Required] ICollection<Equipment> Equipments) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Capacity <= 0)
            {
                yield return new ValidationResult("Вместимость должна быть больше нуля", [nameof(Capacity)]);
            }
        }
    }

    public sealed class Handler(RoomDbContext dbContext, ILogger<Handler> logger) : ICommandHandler<Command, Response>
    {
        public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
        {
            using var scope = logger.BeginScope("CreateRoom");

            var room = new Room(command.Location, command.Name, command.Description, command.Capacity, command.Equipments);

            dbContext.Rooms.Add(room);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(room.Id.Value);
        }
    }
}