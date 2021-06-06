using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace RickAndMorty.Domain.Caches
{
    public interface IFlushableMemoryCache
    {
        void Set<T>(string key, T value);
        bool TryGetValue<T>(string key, out T value);
        void Flush();
    }

    public class FlushableMemoryCache : IFlushableMemoryCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly HashSet<string> _keys;
        
        private static readonly TimeSpan TimeSpanExpiry =  TimeSpan.FromMinutes(5);

        public FlushableMemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _keys = new HashSet<string>();
        }


        public void Set<T>(string key, T value)
        {
            _memoryCache.Set(key, value, TimeSpanExpiry);

            if (!_keys.Contains(key))
            {
                _keys.Add(key);
            }
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }

        public void Flush()
        {
            foreach (var key in _keys)
            {
                _memoryCache.Remove(key);
            }

            _keys.Clear();
        }
    }
}
