using RoomService.Domain.ValueObjects;

namespace RoomService.Domain.Entities;

public class Room
{
    public RoomId Id { get; private set; }
    
    public Address Location { get; private set; }
    
    public string Name { get; private set; }
    
    public int Capacity { get; private set; }
    
    public string? Description { get; private set; }
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime? UpdatedAt { get; private set; }

    public List<Equipment> Equipments { get; set; } = [];

    public Room(Address location, string name, string? description, int capacity, ICollection<Equipment> equipments)
    {
        Id = RoomId.From(Guid.CreateVersion7());
        Location = location;
        Name = name;
        Description = description;
        Capacity = capacity;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        Equipments = equipments.ToList();
    }
    
    protected Room(){}
    
    public void ChangeDetails(string? name, string? description, int? capacity)
    {
        if(name is not null)
            Name = name;
        
        if(description is not null)
            Description = description;
        
        if(capacity is not null)
            Capacity = capacity.Value;
        
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateEquipment(ICollection<EquipmentId> equipmentsIds)
    {
        var incomingSet = equipmentsIds.ToHashSet();
        var removedCount = Equipments.RemoveAll(e => !incomingSet.Contains(e.Id));

        if (removedCount > 0)
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}