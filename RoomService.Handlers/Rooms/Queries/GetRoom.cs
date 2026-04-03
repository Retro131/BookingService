using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoomService.Core.Abstractions.Queries;
using RoomService.Domain.Entities;
using RoomService.Domain.Exceptions;
using RoomService.Domain.ValueObjects;
using RoomService.Infrastructure.Ef;

namespace RoomService.Handlers.Rooms.Queries;

public class GetRoom
{
    public sealed record Response(ResponseDto Room);
    
    public sealed record EquipmentDto(Guid Id, string? Name, string? Description);

    public sealed record ResponseDto(
        Guid RoomId,
        Address Location,
        string Name,
        string? Description,
        int Capacity,
        ICollection<EquipmentDto> Equipments);
    
    public sealed record Query(RoomId RoomId) : IQuery<Response>;

    public sealed class Handler(RoomDbContext dbContext, ILogger<Handler> logger) : IQueryHandler<Query, Response>
    {
        public async Task<Response> HandleAsync(Query query, CancellationToken cancellationToken)
        {
            using var scope = logger.BeginScope("GetRoom");

            var response = await dbContext.Rooms.Where(x => x.Id == query.RoomId && x.IsActive)
                .Select(room => new ResponseDto(
                        room.Id.Value,
                        room.Location,
                        room.Name,
                        room.Description,
                        room.Capacity,
                        room.Equipments.Select(e => new EquipmentDto(
                            e.Id.Value,
                            e.Name,
                            e.Description)
                        ).ToList()
                    )
                )
                .FirstOrDefaultAsync(cancellationToken);

            if (response is null)
            {
                throw new NotFoundException(ErrorMessages.RoomNotFound);
            }

            return new Response(response!);
        }
    }
}