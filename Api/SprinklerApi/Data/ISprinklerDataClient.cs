namespace SprinklerApi.Data
{
    using System;
    using System.Threading.Tasks;
    
    public interface ISprinklerDataClient
    {
        Task<T> CallAsync<T>(Func<SprinklerDbContext, Task<T>> func);
        Task CallAsync(Func<SprinklerDbContext, Task> func);
    }
}
