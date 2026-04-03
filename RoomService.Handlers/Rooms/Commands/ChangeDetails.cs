using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoomService.Core.Abstractions.Commands;
using RoomService.Domain.Entities;
using RoomService.Domain.Exceptions;
using RoomService.Domain.ValueObjects;
using RoomService.Infrastructure.Ef;

namespace RoomService.Handlers.Rooms.Commands;

public class ChangeDetails
{
    public sealed record Response(ResponseDto Room);

    public sealed record ResponseDto(
        Guid RoomId,
        string Name,
        string? Description,
        int Capacity);
    
    public sealed record Command(
        RoomId RoomId,
        string? Name,
        int? Capacity,
        string? Description) : ICommand<Response>;

    public sealed record ChangeDetailRequest(
        [property: MaxLength(64)] string? Name,
        int? Capacity,
        [property: MaxLength(128)] string? Description) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name is null && Capacity is null && Description is null)
            {
                yield return new ValidationResult("Вы не меняете никакие данные");
            }
            
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
            using var scope = logger.BeginScope("ChangeRoomDetails");
            
            var room = await dbContext.Rooms.FindAsync([command.RoomId], cancellationToken);

            if (room is null)
            {
                throw new NotFoundException(ErrorMessages.RoomNotFound);
            }
            
            room!.ChangeDetails(command.Name, command.Description, command.Capacity);
            
            await dbContext.SaveChangesAsync(cancellationToken);
            
            var response = new ResponseDto(room.Id.Value, room.Name, room.Description, room.Capacity);
            
            return new Response(response);
        }
    }
}