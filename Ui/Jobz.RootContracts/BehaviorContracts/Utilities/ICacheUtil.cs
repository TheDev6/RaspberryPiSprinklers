namespace Jobz.RootContracts.BehaviorContracts.Utilities
{
    using System;

    public interface ICacheUtil
    {
        void AddOrSet(string key, object value, int cacheExpireMinutes);
        void ClearAllCache();
        void ClearCacheByKey(string key);
        INullOrDefault<T> Get<T>(string key);
        T GetCacheThenSource<T>(string key, Func<T> source, int cacheExpireMinutes);
    }

    public interface INullOrDefault<T>
    {
        T ValueOrDefault { get; set; }
        bool IsNull { get; set; }
    }
}
