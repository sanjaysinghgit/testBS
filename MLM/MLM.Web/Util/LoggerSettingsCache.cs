using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;

namespace MLM.Util
{
    public class LoggerSettingsCache
    {
        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_connectionString))
                {
                    var setting = ConfigurationManager.ConnectionStrings["Bazinga.Db.Api"];
                    if (setting != null)
                        _connectionString = setting.ConnectionString;  
                }
                return _connectionString;
            }
        }

        public LoggerSettings Create()
        {
            return new LoggerSettings
            {
                RefreshPeriod = 60,
                IsTraceEnabled = false,
                IsDebugEnabled = false,
                IsInfoEnabled = false,
                IsWarnEnabled = true,
                IsErrorEnabled = true,
                IsFatalEnabled = true
            };
        }
        public LoggerSettings GetSettings()
        {
            var cache = MemoryCache.Default;
            var settings = (LoggerSettings)cache.Get("Setting.Application.Logger");
            if (settings != null)
            {
                return settings;
            }

            return Task.Run(() =>
            {
                try
                {
                    settings = Create();

                    using (var conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("select value from dbo.ConfigurationSettingValues csv inner join dbo.ConfigurationSettings cs on cs.Id = csv.ConfigurationSettingId and cs.ConfigurationKey = 'Setting.Application.Logger'", conn))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    settings = JsonConvert.DeserializeObject<LoggerSettings>(reader.GetString(0));
                                    if (settings.RefreshPeriod <= 0)
                                        settings.RefreshPeriod = 60;
                                }
                            }
                        }
                    }
                    cache.Add("Setting.Application.Logger", settings, new CacheItemPolicy { SlidingExpiration = new TimeSpan(0, 0, 0, 30) });
                }
                catch (Exception exc)
                {
                    Logger.Error(exc);
                }

                return settings;
            }).Result;
        }
    }
}