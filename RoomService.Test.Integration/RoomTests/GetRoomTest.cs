using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RoomService.Domain.ValueObjects;
using RoomService.Handlers.Rooms.Commands;
using RoomService.Handlers.Rooms.Queries;
using RoomService.Test.Integration.TestBase;

namespace RoomService.Test.Integration.RoomTests;

[Collection("IntegrationTests")]
public class GetRoomTest(SharedDbFixture dbFixture) : IntegrationTestBase(dbFixture)
{
    [Fact]
    public async Task Get_Room_Ok()
    {
        var logger = Substitute.For<ILogger<GetRoom.Handler>>();
        var handler = new GetRoom.Handler(Context, logger);

        var createdRoom = TestDataFactory.CreateRoom(Context);
        
        var query = new GetRoom.Query(createdRoom.Id);
        
        var result = await handler.HandleAsync(query, CancellationToken.None);
        
        var expectedResponse = new GetRoom.Response(
            new GetRoom.ResponseDto(
                createdRoom.Id.Value,
                createdRoom.Location,
                createdRoom.Name,
                createdRoom.Description,
                createdRoom.Capacity,
                createdRoom.Equipments.Select(e => new GetRoom.EquipmentDto(
                    e.Id.Value,
                    e.Name,
                    e.Description
                )).ToList()
            )
        );
        
        result.Should().BeEquivalentTo(expectedResponse);
    }
}