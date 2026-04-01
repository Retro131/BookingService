using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoomService.Core.Abstractions.Queries;
using RoomService.Infrastructure.Ef;

namespace RoomService.Handlers.EquipmentHandlers.Queries;

public class GetEquipments
{
    public sealed record Response(ICollection<EquipmentDto> Equipments);

    public sealed record EquipmentDto(
        Guid EquipmentId,
        string Name);
    
    public sealed record Query( ) : IQuery<Response>;

    public sealed class Handler(RoomDbContext dbContext, ILogger<Handler> logger) : IQueryHandler<Query, Response>
    {
        public async Task<Response> HandleAsync(Query query, CancellationToken cancellationToken)
        {
            using var scope = logger.BeginScope("GetEquipments");

            var dbQuery = dbContext.Equipments.AsNoTracking();

            var response = await dbQuery.Select(x => new EquipmentDto(
                x.Id.Value,
                x.Name)).ToListAsync(cancellationToken);

            return new Response(response);
        }
    }
}