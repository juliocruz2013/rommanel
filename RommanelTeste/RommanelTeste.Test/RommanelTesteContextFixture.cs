using Microsoft.EntityFrameworkCore;
using RommanelTeste.Application.Common.Interfaces;
using RommanelTeste.Persistence;

namespace RommanelTeste.Test
{
    public class RommanelTesteContextFixture : IDisposable
    {
        public IRommanelTesteContext Context { get; private set; }

        public RommanelTesteContextFixture()
        {
            var options = new DbContextOptionsBuilder<RommanelTesteContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new RommanelTesteContext(options);
            dbContext.Database.EnsureCreated();
            Context = dbContext;
        }

        public void Dispose()
        {
            if (Context is DbContext dbContext)
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Dispose();
            }
        }
    }
}
