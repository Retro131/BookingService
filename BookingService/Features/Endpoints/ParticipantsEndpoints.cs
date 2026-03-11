using BookingService.Services.Interfaces;
using Core.DTO.Booking;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Features.Endpoints;

public static class ParticipantsEndpoints
{
    public static void MapParticipantsEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/invite", async (
            InvitedUsersToBookingInfo invitedUsersToBookingInfo,
            CancellationToken token,
            IBookingService bookingService) =>
        {
            await bookingService.AddParticipantsToBookingAsync(invitedUsersToBookingInfo, token);
        });
        
        group.MapPost("/{bookingId:guid}/decline", async (
            Guid bookingId,
            [FromHeader(Name = "X-User-Id")] Guid userId,
            CancellationToken token,
            IBookingService bookingService) =>
        {
            await bookingService.DeclineBookingAsync(bookingId, userId, token);
        });
        
        group.MapPost("/{bookingId:guid}/accept", async (
            Guid bookingId,
            [FromHeader(Name = "X-User-Id")] Guid userId,
            CancellationToken token,
            IBookingService bookingService) =>
        {
            await bookingService.AcceptBookingAsync(bookingId, userId, token);
        });
    }
}