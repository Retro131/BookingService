using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using RoomService.Domain.ValueObjects;
using RoomService.Endpoints;
using RoomService.ExceptionHandler;
using RoomService.Extensions;
using RoomService.Handlers.Rooms.Queries;
using RoomService.Infrastructure.Ef;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
    options.AddSchemaTransformer((schema, context, cancellationToken) =>
    {
        if (context.JsonTypeInfo.Type == typeof(RoomId))
        {
            schema.Type = JsonSchemaType.String;
            schema.Format = "uuid";
        }
        if (context.JsonTypeInfo.Type == typeof(EquipmentId))
        {
            schema.Type = JsonSchemaType.String;
            schema.Format = "uuid";
        }
        return Task.CompletedTask;
    }));

builder.Services.AddHandlersFromAssembly(typeof(GetRooms.Handler).Assembly);

builder.Services.AddDbContext<RoomDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("RoomDb"));
});

builder.Services.AddExceptionHandler<NotFoundHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(); 
app.MapEndpoints();

app.UseHttpsRedirection();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<RoomDbContext>();
await db.Database.MigrateAsync();

app.Run();
