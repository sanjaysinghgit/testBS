using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web;
using System.Configuration;
using MLM.Logging;

namespace MLM.Util
{
    public class MLMConfig : IMLMConfig
    {
        private const int RefreshIntervalInMinutes = 15;
        private MemoryCache Cache { get { return MemoryCache.Default; } }

        private static readonly HashSet<string> OverridableConfigs = new HashSet<string> { "toggle-navigation" };

        public string GetSetting(string settingName, HttpContextBase context)
        {
            if (OverridableConfigs.Contains(settingName) && context.Request.Cookies.Get(settingName) != null)
            {
                var httpCookie = context.Request.Cookies.Get(settingName);
                if (httpCookie != null)
                {
                    return httpCookie.Value;
                }
            }

            return GetSetting(settingName);
        }

        public string GetSetting(string settingName)
        {
            var settingValue = Cache[settingName] as string;
            if (settingValue == null)
            {
                string source;
                //settingValue = CloudConfigurationManager.GetSetting(settingName);

                if (string.IsNullOrEmpty(settingValue))
                {
                    source = "Web.config";
                    settingValue = ConfigurationManager.AppSettings[settingName];
                }
                else
                {
                    source = "Cloud Config";
                }

                //Logger.LogInfo(new LogEntry
                //{
                //    Message = "Read configuration setting.",
                //    Object = new { settingName = settingName, settingValue = settingValue, source = source }
                //});

                if (!string.IsNullOrEmpty(settingValue))
                    Cache.Set(settingName, settingValue, new CacheItemPolicy { SlidingExpiration = new TimeSpan(0, RefreshIntervalInMinutes, 0) });
            }

            return settingValue;
        }
    }
}