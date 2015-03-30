using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace MLM.UtilityClasses
{
    public static class CommonUtilities
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            string defDesc = string.Empty;
            if (null != fi)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    defDesc = ((DescriptionAttribute)attrs[0]).Description;
            }
            return defDesc;
        }

        public static IEnumerable<T> Clone<T>(this IEnumerable<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        /// <summary>
        /// Gets the Dynamic target url
        /// </summary>
        /// <param name="targetModuleName">Current Module name which is required to get the corresponding hostname</param>
        /// <param name="targetModuleUrlPart">Destination part along with the dynamic value if any</param>
        /// <returns>Target url as string</returns>
        public static string GetTargetURL(string targetModuleName, string targetModuleUrlPart)
        {
            //Note: Empty or null check is not done as there will/should be this key ServerHostName
            return GetServerHostName() + GetHostName(targetModuleName) + targetModuleUrlPart;
        }

        /// <summary>
        /// Gets the server host name
        /// </summary>
        /// <returns>Server host name as string</returns>
        private static string GetServerHostName()
        {
            string serverHostName = Configuration.CustomConfigurationManager.GetConfigurationValue("ServerHostName");
            if (string.IsNullOrEmpty(serverHostName))
            {
                serverHostName = ConfigurationManager.AppSettings["ServerHostName"];
            }
            return serverHostName;
        }

        /// <summary>
        /// Gets the host name based on the target module name
        /// </summary>
        /// <param name="targetModuleName">Current target module name</param>
        /// <returns>Hostname as string</returns>
        private static string GetHostName(string targetModuleName)
        {
            //TODO:: This is tightly coupled. Need to see whether we can remove this coupling, otherwise this would grow 
            //as and when the VD retrieval increases
            string vdName = string.Empty;

            //Note: Empty or null check is not done as there will/should be the below keys

            if (targetModuleName.Equals("modEmploymentportal", StringComparison.OrdinalIgnoreCase))
                vdName = "EmpPortalHostName";
            else if (targetModuleName.Equals("modExperientiallearning", StringComparison.OrdinalIgnoreCase))
                vdName = "EPHostName";

            vdName = Configuration.CustomConfigurationManager.GetConfigurationValue(vdName);

            if (string.IsNullOrEmpty(vdName))
                vdName = ConfigurationManager.AppSettings[vdName];

            return vdName;
        }


    }
}