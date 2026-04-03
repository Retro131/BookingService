using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RoomService.Handlers.Rooms.Commands;
using RoomService.Test.Integration.TestBase;

namespace RoomService.Test.Integration.RoomTests;

[Collection("IntegrationTests")]
public class DeactivateRoomTest(SharedDbFixture dbFixture) : IntegrationTestBase(dbFixture)
{
    [Fact]
    public async Task Deactivate_Room_Ok()
    {
        var logger = Substitute.For<ILogger<DeactivateRoom.Handler>>();
        var handler = new DeactivateRoom.Handler(Context, logger);

        var createdRoom = TestDataFactory.CreateRoom(Context);
        
        var command = new DeactivateRoom.Command(
            createdRoom.Id
        );

        var result = await handler.HandleAsync(command, CancellationToken.None);

        result.Should().NotBeNull();
        
        var roomInDb = await Context.Rooms.FindAsync(createdRoom.Id);
        
        roomInDb.Should().NotBeNull();
        roomInDb.IsActive.Should().Be(false);
    }
}