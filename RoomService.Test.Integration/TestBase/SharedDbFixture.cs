using Microsoft.EntityFrameworkCore;
using RoomService.Infrastructure.Ef;
using Testcontainers.PostgreSql;

namespace RoomService.Test.Integration;

public class SharedDbFixture : IAsyncLifetime 
{
    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder("postgres:15-alpine")
        .Build();
    
    public string ConnectionString => _dbContainer.GetConnectionString();
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}