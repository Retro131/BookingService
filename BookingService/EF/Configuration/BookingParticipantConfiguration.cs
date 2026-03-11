using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.EF.Configuration;

public class BookingParticipantConfiguration : IEntityTypeConfiguration<BookingParticipant>
{
    public void Configure(EntityTypeBuilder<BookingParticipant> modelBuilder)
    {
        modelBuilder.HasKey(bP => new { bP.UserId, bP.BookingId });
        modelBuilder.Property(bP => bP.Status).IsRequired();
        modelBuilder.Property(bP => bP.Role).IsRequired();
    }
}