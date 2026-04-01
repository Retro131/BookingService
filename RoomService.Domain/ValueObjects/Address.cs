namespace RoomService.Domain.ValueObjects;

public record Address
{
    public string Street { get; }
    public string Building { get; }
    public string City { get; }

    public Address(string street, string building, string city)
    {
        if (string.IsNullOrWhiteSpace(street)) throw new ArgumentNullException(nameof(street));
        if (string.IsNullOrWhiteSpace(building)) throw new ArgumentNullException(nameof(building));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentNullException(nameof(city));
        
        Street = street;
        Building = building;
        City = city;
    }
}