using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomService.Domain.Entities;
using RoomService.Domain.ValueObjects;

namespace RoomService.Infrastructure.Ef.Configuration;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).HasConversion<EquipmentId.EfCoreValueConverter>();
    }
}