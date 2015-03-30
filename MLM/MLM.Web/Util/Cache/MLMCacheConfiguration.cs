using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace MLM.Util.Cache
{
    public class MLMCacheConfiguration
    {
        private const string CacheKeyForFile = "bazingaCacheConfiguration";
        private const string CachedFilePath = "~/App_Data/ApiCache/Configuration.xml";

        public static MLMCache CheckRouteCacheValidity(string pathAndQuery)
        {
            var cacheConfigList = GetCacheConfigurationItems();
            if (!cacheConfigList.Any())
            {
                return null;
            }

            var cacheItem = cacheConfigList.FirstOrDefault(c => Regex.IsMatch(pathAndQuery, c.Match, RegexOptions.IgnoreCase));
            if (cacheItem == null)
            {
                return null;
            }

            var bazingaCache = new MLMCache
            {
                CacheAge = cacheItem.CacheAge,
                CacheKey = pathAndQuery
            };

            return bazingaCache;
        }
        
        private static List<CacheConfigurationData> GetCacheConfigurationItems()
        {
            var bazingaCachingWrapper = new MLMCachingWrapper();
            var cacheConfigs = bazingaCachingWrapper.GetCachedItem(CacheKeyForFile) as List<CacheConfigurationData>;
            if (cacheConfigs != null)
            {
                return cacheConfigs;
            }

            var configXDocument = GetConfigurationData();
            if (configXDocument == null)
            {
                return new List<CacheConfigurationData>();
            }

            var cacheConfigList = new List<CacheConfigurationData>();
            var items = configXDocument.Descendants("cacheModule");
            foreach (var item in items)
            {
                var cacheConfiguration = new CacheConfigurationData();
                var matchText = item.Attribute("match");
                if (matchText != null)
                {
                    cacheConfiguration.Match = matchText.Value.Trim();
                }

                var cacheAge = item.Attribute("cacheAge");
                if (cacheAge != null)
                {
                    cacheConfiguration.CacheAge = int.Parse(cacheAge.Value.Trim());
                }

                var keyPrefix = item.Attribute("keyPrefix");
                if (keyPrefix != null)
                {
                    cacheConfiguration.KeyPrefix = keyPrefix.Value;
                }
                cacheConfigList.Add(cacheConfiguration);
            }

            bazingaCachingWrapper.AddToCache(
                CacheKeyForFile, 
                1, 
                cacheConfigList, 
                CacheItemPriority.Default,
                new[] { HttpContext.Current.Server.MapPath(CachedFilePath) });

            return cacheConfigList;
        }

        private static XDocument GetConfigurationData()
        {
            var cachedPhysicalFilePath = HttpContext.Current.Server.MapPath(CachedFilePath);
            return File.Exists(cachedPhysicalFilePath) ? XDocument.Load(cachedPhysicalFilePath) : null;
        }

        private class CacheConfigurationData
        {
            public string Match { get; set; }
            public int CacheAge { get; set; }
            public string KeyPrefix { get; set; }
        }
    }
}