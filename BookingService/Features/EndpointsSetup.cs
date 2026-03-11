using BookingService.Features.Endpoints;

namespace BookingService.Features;

public static class EndpointsSetup
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var bookings = app
            .MapGroup("/api/v1/bookings")
            .WithTags("Bookings");

        var participants = app
            .MapGroup("/api/v1/participants")
            .WithTags("Participants");
        
        bookings.MapBookingEndpoints();
        participants.MapParticipantsEndpoints();

        return app;
    }
}