namespace SprinklerApi.Data
{
    using System;
    using System.Threading.Tasks;

    public class SprinklerDataClient : ISprinklerDataClient
    {
        private readonly SprinklerDbContext _context;
        public SprinklerDataClient(SprinklerDbContext context)
        {
            _context = context;
        }

        public async Task<T> CallAsync<T>(Func<SprinklerDbContext, Task<T>> func)
        {
            return await func(_context);
        }

        public async Task CallAsync(Func<SprinklerDbContext, Task> func)
        {
            await func(_context);
        }
    }
}
