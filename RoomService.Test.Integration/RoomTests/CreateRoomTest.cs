using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RoomService.Domain.ValueObjects;
using RoomService.Handlers.Rooms.Commands;
using RoomService.Test.Integration.TestBase;

namespace RoomService.Test.Integration.RoomTests;

[Collection("IntegrationTests")]
public class CreateRoomTest(SharedDbFixture dbFixture) : IntegrationTestBase(dbFixture)
{
    [Fact]
    public async Task Create_Room_Ok()
    {
        var logger = Substitute.For<ILogger<CreateRoom.Handler>>();
        var handler = new CreateRoom.Handler(Context, logger);
        
        var command = new CreateRoom.Command(
            Name: "Тестовая",
            Location: new Address(city: "Тестовая", building: "тестовое", street:"тестовая"),
            Capacity: 20,
            Description: "Для теста",
            Equipments: []
            );

        var result = await handler.HandleAsync(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        
        var roomInDb = await Context.Rooms.FindAsync([RoomId.From(result.Id)]);
        roomInDb.Should().NotBeNull();
        roomInDb!.Name.Should().Be("Тестовая");
        roomInDb.Capacity.Should().Be(20);
        roomInDb.Description.Should().Be("Для теста");
    }
}