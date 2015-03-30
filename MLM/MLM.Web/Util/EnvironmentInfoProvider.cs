using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;

namespace MLM.Util
{
    public class EnvironmentInfoProvider
    {
        private readonly IMLMConfig _bazingaConfig;

        public EnvironmentInfoProvider(IMLMConfig bazingaConfig)
        {
            _bazingaConfig = bazingaConfig;
        }

        public EnvironmentInfo GatherInfo()
        {
            EnvironmentInfo info = new EnvironmentInfo();
            info.AssemblyVersion = GetAssemblyVersion();
            info.Environment = _bazingaConfig.GetSetting("Environment");
            info.OSName = Environment.OSVersion.VersionString;
            info.MachineName = Environment.MachineName;
            info.Bitness = Environment.Is64BitOperatingSystem ? "64 bits" : "32 bits";
            var parameters = new [] {
                "ApiBasePath",
                "ApiCaching",
                "ApiMocking",
                "ApiUrl",
                "AppInsightsClientKey",
                "AuthCookieName",
                "AuthorizationHeaderName",
                "AuthUrl",
                "Bazinga-Header",
                "ClientId",
                "ClientSecret",
                "ConnectionString.mlm.Db.Api",
                "ConnectionString.mlm.Db.Logging",
                "Environment",
                "ExpireTimeCookieName",
                "ForceSSL",
                "GitRevisionNumber",
                "GoogleAnalyticsKey",
                "Microsoft.AppInsights.AccountId",
                "Microsoft.AppInsights.DisplayName",
                "Microsoft.AppInsights.EnableMonitoring",
                "Microsoft.AppInsights.InstrumentationKey",
                "MockApiUrl",
                "OauthRealm",
                "profileUrl",
                "ProxyBasePath",
                "ProxyBasePathAnonymous",
                "ProxyBasePathSecuredFile",
                "RefreshTokenCookieName",
                "TeamCityBuildNumber",
                "toggle-navigation",
                "ValidIssuer"
            };

            info.Extra = string.Join("<br />\r\n", parameters.Select(x => x + ": " + _bazingaConfig.GetSetting(x)));

            return info;
        }

        private static string GetAssemblyVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            return version;
        }
    }
}