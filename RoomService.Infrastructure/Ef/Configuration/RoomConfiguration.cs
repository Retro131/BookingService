using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomService.Domain.Entities;
using RoomService.Domain.ValueObjects;

namespace RoomService.Infrastructure.Ef.Configuration;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).HasConversion<RoomId.EfCoreValueConverter>();
        builder.Property(x => x.Capacity).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        
        builder.ComplexProperty(x => x.Location, addressBuilder =>
        {
            addressBuilder.Property(x => x.Street).IsRequired();
            addressBuilder.Property(x => x.Building).IsRequired();
            addressBuilder.Property(x => x.City).IsRequired();
        });
        
        builder.HasMany(x => x.Equipments)
            .WithMany(x => x.Rooms)
            .UsingEntity(j => j.ToTable("RoomEquipment"));
    }
}