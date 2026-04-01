using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using RoomService.Core.Abstractions.Commands;
using RoomService.Domain.Entities;
using RoomService.Infrastructure.Ef;

namespace RoomService.Handlers.EquipmentHandlers.Commands;

public class AddEquipment
{
    public sealed record Response(Guid Id);

    public sealed record Command(
        string Name,
        string? Description) : ICommand<Response>;

    public sealed record AddEquipmentRequest(
        [property: MaxLength(64)] string Name,
        [property: MaxLength(128)] string? Description)
    {
    }

    public sealed class Handler(RoomDbContext dbContext, ILogger<Handler> logger) : ICommandHandler<Command, Response>
    {
        public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
        {
            using var scope = logger.BeginScope("AddEquipment");

            var equipment = new Equipment(command.Name, command.Description);
            
            dbContext.Equipments.Add(equipment);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(equipment.Id.Value);
        }
    }
}