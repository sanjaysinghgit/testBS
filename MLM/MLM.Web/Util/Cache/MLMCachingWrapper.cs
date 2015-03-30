using System;
using System.IO;
using System.Linq;
using System.Runtime.Caching;

namespace MLM.Util.Cache
{
    public class MLMCachingWrapper
    {
        // Gets a reference to the default MemoryCache instance. 
        private static readonly ObjectCache Cache = MemoryCache.Default;
        private CacheItemPolicy _policy = null;
        private CacheEntryRemovedCallback _callback = null;

        public void AddToCache(string cacheKey, int cacheAge, Object cacheItem, CacheItemPriority cacheItemPriority, params string[] filePaths)
        {
            _callback = new CacheEntryRemovedCallback(this.CachedItemRemovedCallback);
            _policy = new CacheItemPolicy
                {
                    Priority = cacheItemPriority,
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheAge),
                    RemovedCallback = _callback
                };

            // Invalidate cache if files are modified
            if (filePaths.Any())
            {
                _policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));
            }

            // Add inside cache 
            Cache.Set(cacheKey, cacheItem, _policy);
        }

        public Object GetCachedItem(String cacheKeyName)
        {
            return Cache[cacheKeyName] as Object;
        }

        public void RemoveCachedItem(String cacheKeyName)
        {
            if (Cache.Contains(cacheKeyName))
            {
                Cache.Remove(cacheKeyName);
            }
        }

        private void CachedItemRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            // Log these values from arguments list 
            String strLog = String.Concat("Reason: ", arguments.RemovedReason.ToString(), "Key-Name: ", arguments.CacheItem.Key, " | Value-Object: ", arguments.CacheItem.Value.ToString());
        }
    }
}