using Microsoft.AspNetCore.Mvc;
using RoomService.Core.Abstractions.Commands;
using RoomService.Core.Abstractions.Queries;
using RoomService.Domain.ValueObjects;
using RoomService.Handlers.Rooms.Commands;
using RoomService.Handlers.Rooms.Queries;

namespace RoomService.Endpoints;

public static class RoomEndpoints
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var rooms = app
            .MapGroup("/api/v1/rooms")
            .WithTags("Rooms");

        rooms.MapPost("/", async (
            [FromBody] CreateRoom.CreateRoomRequest createRoomRequest,
            [FromServices] ICommandHandler<CreateRoom.Command, CreateRoom.Response> handler,
            CancellationToken token) =>
        {
            var command = new CreateRoom.Command(
                createRoomRequest.Name,
                createRoomRequest.Location,
                createRoomRequest.Capacity,
                createRoomRequest.Description,
                createRoomRequest.Equipments);

            var response = await handler.HandleAsync(command, token);

            return Results.Ok(response);
        });

        rooms.MapPost("/{roomId:guid}/activate", async (
            [FromRoute] Guid roomId,
            [FromServices] ICommandHandler<ActivateRoom.Command, ActivateRoom.Response> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new ActivateRoom.Command(RoomId.From(roomId));

            var response = await handler.HandleAsync(command, cancellationToken);

            return Results.Ok(response);
        });

        rooms.MapPost("/{roomId:guid}/deactivate", async (
            [FromRoute] Guid roomId,
            [FromServices] ICommandHandler<DeactivateRoom.Command, DeactivateRoom.Response> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeactivateRoom.Command(RoomId.From(roomId));

            var response = await handler.HandleAsync(command, cancellationToken);

            return Results.Ok(response);
        });

        rooms.MapPatch("/{roomId:guid}/", async (
            [FromRoute] Guid roomId,
            [FromBody] ChangeDetails.ChangeDetailRequest changeDetailsRequest,
            [FromServices] ICommandHandler<ChangeDetails.Command, ChangeDetails.Response> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new ChangeDetails.Command(
                RoomId.From(roomId), 
                changeDetailsRequest.Name,
                changeDetailsRequest.Capacity,
                changeDetailsRequest.Description);

            var response = await handler.HandleAsync(command, cancellationToken);

            return Results.Ok(response);
        });

        rooms.MapPatch("/{roomId:guid}/equipment", async (
            [FromRoute] Guid roomId,
            [FromBody] UpdateEquipment.UpdateEquipmentRequest updateEquipmentRequest,
            [FromServices] ICommandHandler<UpdateEquipment.Command, UpdateEquipment.Response> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateEquipment.Command(
                RoomId.From(roomId), 
                updateEquipmentRequest.EquipmentIds);

            var response = await handler.HandleAsync(command, cancellationToken);

            return Results.Ok(response);
        });

        rooms.MapGet("/{roomId:guid}", async (
            [FromRoute] Guid roomId,
            [FromServices] IQueryHandler<GetRoom.Query, GetRoom.Response> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetRoom.Query(RoomId.From(roomId));

            var response = await handler.HandleAsync(query, cancellationToken);

            return Results.Ok(response);
        });

        rooms.MapGet("/", async (
            [FromBody] GetRooms.GetRoomsRequest getRoomsRequest,
            [FromServices] IQueryHandler<GetRooms.Query, GetRooms.Response> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetRooms.Query(
                getRoomsRequest.Location,
                getRoomsRequest.Capacity,
                getRoomsRequest.EquipmentIds.Select(EquipmentId.From).ToList());

            var response = await handler.HandleAsync(query, cancellationToken);

            return Results.Ok(response);
        });

        return app;
    }
}