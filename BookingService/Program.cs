using BookingService.EF;
using BookingService.Features;
using BookingService.Repositories;
using BookingService.Repositories.Interfaces;
using BookingService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService.Services.BookingService>();

builder.Services.AddDbContext<BookingDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("BookingDb"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
    o.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.MapEndpoints();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
await db.Database.MigrateAsync();

app.Run();