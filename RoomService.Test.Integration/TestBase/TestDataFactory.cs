using RoomService.Domain.Entities;
using RoomService.Domain.ValueObjects;
using RoomService.Infrastructure.Ef;

namespace RoomService.Test.Integration.TestBase;

public static class TestDataFactory
{
    public static Room CreateRoom(
        RoomDbContext context,
        string name = "Тестовая",
        int capacity = 10,
        bool isActive = true,
        Address address = null)
    {
        var equipment = new Equipment("aa", "aa");
        
        var roomAddr = address is null ? new Address(city: "Seattle", building: "12", street: "Abama") : address;
        
        var room = new Room(
            roomAddr,
            name,
            "",
            capacity,
            [equipment]);

        if (!isActive)
            room.Deactivate();

        context.Rooms.Add(room);
        context.Equipments.Add(equipment);
        context.SaveChanges();

        return room;
    }
}