using RoomService.Domain.ValueObjects;

namespace RoomService.Domain.Entities;

public class Equipment
{
    public EquipmentId Id { get; private set; }
    
    public string Name { get; private set; }
    
    public string? Description { get; private set; }
    
    public List<Room> Rooms { get; set; } = [];

    public Equipment(string name, string? description)
    {
        Id = EquipmentId.From(Guid.CreateVersion7());
        Name = name;
        Description = description;
    }

    protected Equipment(){}
    
    public void ChangeDetails(string? description, string? name)
    {
        if(description is not null)
            Description = description;
        if(name is not null)
            Name = name;
    }
}