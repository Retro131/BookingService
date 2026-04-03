using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using RoomService.Core.Abstractions.Commands;
using RoomService.Domain.Exceptions;
using RoomService.Domain.ValueObjects;
using RoomService.Infrastructure.Ef;

namespace RoomService.Handlers.Rooms.Commands;

public class ActivateRoom
{
    public sealed record Response();

    public sealed record Command(
        [property: Required]
        RoomId RoomId) : ICommand<Response>; 
    
    public sealed class Handler(RoomDbContext dbContext, ILogger<Handler> logger) : ICommandHandler<Command, Response>
    {
        public async Task<Response> HandleAsync(Command command, CancellationToken cancellationToken)
        {
            using var scope = logger.BeginScope("ActivateRoom");
            
            var room = await dbContext.Rooms.FindAsync([command.RoomId], cancellationToken);

            if (room is null)
            {
                throw new NotFoundException(ErrorMessages.RoomNotFound);
            }
            
            room!.Activate();
            
            await dbContext.SaveChangesAsync(cancellationToken);
            
            return new Response();
        }
    }
}