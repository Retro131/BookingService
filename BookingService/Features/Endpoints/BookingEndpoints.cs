using BookingService.Services.Interfaces;
using Core.DTO.Booking;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Features.Endpoints;

public static class BookingEndpoints
{
    public static void MapBookingEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (
            CreateBookingInfo bookingInfo,
            [FromHeader(Name = "X-User-Id")] Guid userId,
            CancellationToken token,
            IBookingService bookingService) =>
        {
            // ReSharper disable once ConvertToLambdaExpression
            return await bookingService.CreateBookingAsync(bookingInfo, userId, token);
        });
        
        group.MapPost("/{bookingId:guid}/cancel", async (
            Guid bookingId,
            [FromHeader(Name = "X-User-Id")] Guid userId,
            CancellationToken token,
            IBookingService bookingService) =>
        {
            // ReSharper disable once ConvertToLambdaExpression
            await bookingService.CancelBooking(bookingId, userId, token);
        });
        
        group.MapGet("/{roomId:guid}", async (
            Guid roomId,
            CancellationToken token,
            IBookingService bookingService) =>
        {
            // ReSharper disable once ConvertToLambdaExpression
            return await bookingService.GetBookingsByRoomId(roomId, token);
        });
    }
}