using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RoomService.Domain.ValueObjects;
using RoomService.Handlers.Rooms.Commands;
using RoomService.Test.Integration.TestBase;

namespace RoomService.Test.Integration.RoomTests;

[Collection("IntegrationTests")]
public class UpdateDetailsTest(SharedDbFixture dbFixture) : IntegrationTestBase(dbFixture)
{
    [Fact]
    public async Task Change_Details_Ok()
    {
        var logger = Substitute.For<ILogger<ChangeDetails.Handler>>();
        var handler = new ChangeDetails.Handler(Context, logger);

        var createdRoom = TestDataFactory.CreateRoom(Context);
        
        var command = new ChangeDetails.Command(
            createdRoom.Id,
            "Новое имя",
            Capacity: 15,
            Description: "новый тест"
        );

        var result = await handler.HandleAsync(command, CancellationToken.None);

        result.Should().NotBeNull();
        
        var roomInDb = await Context.Rooms.FindAsync(createdRoom.Id);
        
        roomInDb.Should().NotBeNull();
        roomInDb!.Name.Should().Be("Новое имя");
        roomInDb.Capacity.Should().Be(15);
        roomInDb.Description.Should().Be("новый тест");
    }
}