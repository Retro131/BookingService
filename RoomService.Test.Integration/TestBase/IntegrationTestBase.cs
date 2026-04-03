using Microsoft.EntityFrameworkCore;
using RoomService.Infrastructure.Ef;

namespace RoomService.Test.Integration.TestBase;

public class IntegrationTestBase(SharedDbFixture dbFixture) : IAsyncLifetime
{
    protected RoomDbContext Context { get; private set; } = null!;
    
    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<RoomDbContext>()
            .UseNpgsql(dbFixture.ConnectionString)
            .Options;

        Context = new RoomDbContext(options);

        await Context.Database.EnsureCreatedAsync();
    }

    public Task DisposeAsync()
    {
        Context.Dispose();
        return Task.CompletedTask;
    }
}