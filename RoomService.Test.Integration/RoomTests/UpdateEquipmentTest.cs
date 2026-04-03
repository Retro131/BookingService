using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RoomService.Domain.Entities;
using RoomService.Handlers.Rooms.Commands;
using RoomService.Test.Integration.TestBase;

namespace RoomService.Test.Integration.RoomTests;

[Collection("IntegrationTests")]
public class UpdateEquipmentTest(SharedDbFixture dbFixture) : IntegrationTestBase(dbFixture)
{
    [Fact]
    public async Task Update_Equipment_Ok()
    {
        var logger = Substitute.For<ILogger<UpdateEquipment.Handler>>();
        var handler = new UpdateEquipment.Handler(Context, logger);

        var createdRoom = TestDataFactory.CreateRoom(Context);
        var newEquipment = new Equipment("bb", "bb");

        Context.Equipments.Add(newEquipment);

        await Context.SaveChangesAsync(CancellationToken.None);
        
        var command = new UpdateEquipment.Command(
            createdRoom.Id,
            [newEquipment.Id]
        );

        var result = await handler.HandleAsync(command, CancellationToken.None);

        result.Should().NotBeNull();

        Context.ChangeTracker.Clear();
        
        var roomInDb = await Context.Rooms
            .Include(x => x.Equipments)
            .FirstOrDefaultAsync(x => x.Id == createdRoom.Id, CancellationToken.None);

        roomInDb.Should().NotBeNull();
        
        roomInDb.Equipments.Should().HaveCount(1);
        roomInDb.Equipments.Should().Contain(e => e.Id == newEquipment.Id);
    }
}