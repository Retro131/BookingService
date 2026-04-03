using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RoomService.Handlers.Rooms.Commands;
using RoomService.Test.Integration.TestBase;

namespace RoomService.Test.Integration.RoomTests;

[Collection("IntegrationTests")]
public class ActivateRoomTest(SharedDbFixture dbFixture) : IntegrationTestBase(dbFixture)
{
    [Fact]
    public async Task Activate_Room_Ok()
    {
        var logger = Substitute.For<ILogger<ActivateRoom.Handler>>();
        var handler = new ActivateRoom.Handler(Context, logger);

        var createdRoom = TestDataFactory.CreateRoom(Context, isActive: false);
        
        var command = new ActivateRoom.Command(
            createdRoom.Id
        );

        var result = await handler.HandleAsync(command, CancellationToken.None);

        result.Should().NotBeNull();
        
        var roomInDb = await Context.Rooms.FindAsync(createdRoom.Id);
        
        roomInDb.Should().NotBeNull();
        roomInDb.IsActive.Should().Be(true);
    }
}