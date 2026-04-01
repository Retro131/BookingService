using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using RoomService.Core.Abstractions.Commands;
using RoomService.Domain.ValueObjects;
using RoomService.Infrastructure.Ef;

namespace RoomService.Handlers.Rooms.Commands;

public class DeactivateRoom
{
    public sealed record Response();

    public sealed record Command(
        [property: Required]
        RoomId RoomId) : ICommand<Response>; 
    
    public sealed class Handler(RoomDbContext dbContext, ILogger<Handler> logger) : ICommandHandler<Command, Response>
    {
        public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
        {
            using var scope = logger.BeginScope("DeactivateRoom");
            
            var room = await dbContext.Rooms.FindAsync([command.RoomId.Value], cancellationToken);

            if (room is null)
            {
                //TODO handle exception
            }
            
            room!.Deactivate();
            
            await dbContext.SaveChangesAsync(cancellationToken);
            
            return new Response();
        }
    }
}