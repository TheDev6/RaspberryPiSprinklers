namespace Jobz.WebUi.Utilities
{
    using System;
    using System.Runtime.Caching;
    using RootContracts.BehaviorContracts.Utilities;

    public class CacheUtil : ICacheUtil
    {
        public INullOrDefault<T> Get<T>(string key)
        {
            //Null or default class exists because I was caching a boolean and it was comming back false even though it had expired and was null, since default T was false, it was breaking premissions
            var cached = MemoryCache.Default.Get(key: key);
            var valueOrDefault = cached != null ? (T)cached : default(T);
            var result = new NullOrDefault<T>()
            {
                IsNull = cached == null,
                ValueOrDefault = valueOrDefault
            };
            return result;
        }

        public void AddOrSet(string key, object value, int cacheExpireMinutes)
        {
            var policy = this.BuildCachePolicy(cacheExpireMinutes);
            if (MemoryCache.Default.Contains(key: key))
            {
                MemoryCache.Default.Set(key: key, value: value, policy: policy);
            }
            else
            {
                MemoryCache.Default.Add(key: key, value: value, policy: policy);
            }
        }

        public T GetCacheThenSource<T>(string key, Func<T> source, int cacheExpireMinutes)
        {
            T result = default(T);
            var cached = this.Get<T>(key: key);
            if (cached.IsNull)
            {
                result = source();
                if (result != null)
                {
                    //do i need to lock stuff here?? Worst case we add twice or error?
                    this.AddOrSet(key: key, value: result, cacheExpireMinutes: cacheExpireMinutes);
                    //add or get existing is very interesting as well and could be a better choice
                }
            }
            else
            {
                result = cached.ValueOrDefault;
            }
            return result;
        }

        public void ClearCacheByKey(string key)
        {
            if (MemoryCache.Default.Contains(key))
            {
                MemoryCache.Default.Remove(key: key);
            }
        }

        public void ClearAllCache()
        {
            foreach (var item in MemoryCache.Default)
            {
                MemoryCache.Default.Remove(key: item.Key);
            }
        }

        private CacheItemPolicy BuildCachePolicy(int cacheExpireMinutes)
        {
            return new CacheItemPolicy()
            {
                SlidingExpiration = new TimeSpan(days: 0, hours: 0, minutes: cacheExpireMinutes, seconds: 0)
            };
        }
    }

    public class NullOrDefault<T> : INullOrDefault<T>
    {
        public T ValueOrDefault { get; set; }
        public bool IsNull { get; set; }
    }
}