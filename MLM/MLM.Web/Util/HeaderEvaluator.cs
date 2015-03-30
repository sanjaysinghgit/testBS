using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;

namespace MLM.Util
{
    public static class HeaderEvaluator
    {
        private static IMLMConfig _bazingaConfig;

        static HeaderEvaluator()
        {
            _bazingaConfig = new MLMConfig();
        }

        public static string GetBazingaHeader(HttpResponseMessage response)
        {
            IEnumerable<string> values;
            if (response.Headers.TryGetValues(_bazingaConfig.GetSetting("Bazinga-Header"), out values))
            {
                return values.FirstOrDefault();
            }

            return string.Empty;
        }
    }
}