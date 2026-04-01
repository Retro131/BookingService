using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoomService.Core.Abstractions.Queries;
using RoomService.Domain.ValueObjects;
using RoomService.Infrastructure.Ef;

namespace RoomService.Handlers.Rooms.Queries;

public class GetRooms
{
    public sealed record Response(ICollection<ShortRoomDto> Rooms);

    public sealed record ShortRoomDto(
        Guid RoomId,
        string Name,
        int Capacity,
        bool IsActive);

    public sealed record GetRoomsRequest(
        [property: Required] Address Location,
        int? Capacity,
        ICollection<Guid> EquipmentIds) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Capacity <= 0)
            {
                yield return new ValidationResult("Вместимость должна быть больше нуля", [nameof(Capacity)]);
            }
        }
    }

    public sealed record Query(
        Address Location,
        int? Capacity,
        ICollection<EquipmentId>? EquipmentIds) : IQuery<Response>;

    public sealed class Handler(RoomDbContext dbContext, ILogger<Handler> logger) : IQueryHandler<Query, Response>
    {
        public async Task<Response> HandleAsync(Query query, CancellationToken cancellationToken)
        {
            using var scope = logger.BeginScope("GetRooms");

            var dbQuery = dbContext.Rooms.AsNoTracking()
                .Where(x => x.Location == query.Location);

            if (query.Capacity is not null)
            {
                dbQuery = dbQuery.Where(x => x.Capacity >= query.Capacity);
            }

            if (query.EquipmentIds is { Count: > 0 })
            {
                dbQuery = dbQuery.Where(x =>
                    x.Equipments.Count(y => query.EquipmentIds.Contains(y.Id)) == query.EquipmentIds.Count);
            }

            var response = await dbQuery.Select(x => new ShortRoomDto(
                x.Id.Value,
                x.Name,
                x.Capacity,
                x.IsActive)).ToListAsync(cancellationToken);

            return new Response(response);
        }
    }
}