using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RoomService.Domain.Entities;
using RoomService.Domain.ValueObjects;
using RoomService.Handlers.Rooms.Queries;
using RoomService.Test.Integration.TestBase;

namespace RoomService.Test.Integration.RoomTests;

[Collection("IntegrationTests")]
public class GetRoomsTest(SharedDbFixture dbFixture) : IntegrationTestBase(dbFixture)
{
    [Fact]
    public async Task Get_RoomsByAddress_Ok()
    {
        var logger = Substitute.For<ILogger<GetRooms.Handler>>();
        var handler = new GetRooms.Handler(Context, logger);

        var rooms = GetTestRooms();

        var query = new GetRooms.Query(new Address("1", "1", "1"), null, null);

        var result = await handler.HandleAsync(query, CancellationToken.None);

        var expectedRooms = rooms
            .Where(r => r.Location == new Address("1", "1", "1"))
            .Select(r => new GetRooms.ShortRoomDto(
                r.Id.Value,
                r.Name,
                r.Capacity,
                r.IsActive));

        result.Rooms.Should().BeEquivalentTo(expectedRooms);
    }

    [Fact]
    public async Task Get_RoomsByCapacity_Ok()
    {
        var logger = Substitute.For<ILogger<GetRooms.Handler>>();
        var handler = new GetRooms.Handler(Context, logger);

        var rooms = GetTestRoomsOnSameAddress();

        var query = new GetRooms.Query(rooms.First().Location, 10, null);

        var result = await handler.HandleAsync(query, CancellationToken.None);

        var expectedRooms = rooms
            .Where(r => r.Capacity >= 10)
            .Select(r => new GetRooms.ShortRoomDto(
                r.Id.Value,
                r.Name,
                r.Capacity,
                r.IsActive));

        result.Rooms.Should().BeEquivalentTo(expectedRooms);
    }

    private ICollection<Room> GetTestRooms()
    {
        return
        [
            TestDataFactory.CreateRoom(
                Context,
                address: new Address("1", "1", "1"),
                capacity: 10,
                isActive: true),
            TestDataFactory.CreateRoom(
                Context,
                address: new Address("2", "2", "2"),
                capacity: 10,
                isActive: true),
            TestDataFactory.CreateRoom(
                Context,
                address: new Address("3", "3", "3"),
                capacity: 10,
                isActive: true),
            TestDataFactory.CreateRoom(
                Context,
                address: new Address("3", "3", "3"),
                capacity: 20,
                isActive: true),
            TestDataFactory.CreateRoom(
                Context,
                address: new Address("3", "3", "3"),
                capacity: 2,
                isActive: false),
            TestDataFactory.CreateRoom(
                Context,
                address: new Address("1", "1", "1"),
                capacity: 15,
                isActive: false),
            TestDataFactory.CreateRoom(
                Context,
                address: new Address("1", "1", "1"),
                capacity: 15),
        ];
    }

    private ICollection<Room> GetTestRoomsOnSameAddress()
    {
        return
        [
            TestDataFactory.CreateRoom(
                Context,
                capacity: 10,
                address: new Address("4", "4", "4"),
                isActive: true),
            TestDataFactory.CreateRoom(
                Context,
                capacity: 10, address: new Address("4", "4", "4"),
                isActive: true),
            TestDataFactory.CreateRoom(
                Context,
                capacity: 10, address: new Address("4", "4", "4"),
                isActive: true),
            TestDataFactory.CreateRoom(
                Context,
                capacity: 20, address: new Address("4", "4", "4"),
                isActive: true),
            TestDataFactory.CreateRoom(
                Context,
                capacity: 2, address: new Address("4", "4", "4"),
                isActive: false),
            TestDataFactory.CreateRoom(
                Context,
                capacity: 15, address: new Address("4", "4", "4"),
                isActive: false),
            TestDataFactory.CreateRoom(
                Context,
                capacity: 15, address: new Address("4", "4", "4")
            ),
        ];
    }
}