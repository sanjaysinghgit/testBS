using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Globalization;
using EPS.IoC.ServiceLocation;
using Microsoft.Practices.ServiceLocation;

namespace MLM.Configuration
{
    /// <summary>
    /// MLM Framework class for retrieving Configuration from the current appDomain's configuration file.
    /// </summary>
    public static class CustomConfigurationManager
    {
        private const string DefaultSection = "appSettings";

        // TODO: Performance Issues?
        // TODO: To be made public when fully ready
        private static T GetModuleConfiguration<T>(string key, T defaultValue) where T : IConvertible
        {
            if (string.IsNullOrEmpty(key))
                return defaultValue;

            var contextProvider = CommonBootStrapper.Locator.GetInstance<IApplicationContextProvider>();
            if (contextProvider == null)
                return defaultValue;

            var moduleName = contextProvider.GetContextObject<string, string>(Constants.ContextModuleKey);
            var sectionName = DefaultSection;

            // Take module section or DefaultSection
            if (!string.IsNullOrEmpty(moduleName))
                sectionName = string.Format("{0}{1}", Constants.ModuleConfigSectionPrefix,
                    new CultureInfo("en-US").TextInfo.ToTitleCase(moduleName));

            return GetConfigurationValue(sectionName, key, defaultValue);
        }

        public static string GetModuleConfiguration(string key, string defaultValue)
        {
            return GetModuleConfiguration<string>(key, defaultValue);
        }

        public static string GetModuleConfiguration(string key)
        {
            // Default is picking directly from main app.config, if module configuration is not found
            return GetModuleConfiguration(key, GetConfigurationValue(key));
        }

        /// <summary>
        /// Retrieves configuration value for <param name="key">key</param> from configuration section 
        /// name passed into <param name="section">section</param>
        /// </summary>
        /// <param name="section">Configuration Section Specified in the Configuration File. Should be of type <code>System.Configuration.AppSettingsSection</code> 
        /// that allows appSettings like name value pairs for configuration.</param>
        /// <param name="key">Configuration Key in the specified section.</param>
        /// <returns>Configuration Value for section and key specified. If key is not found, returns <code>String.Empty</code>.</returns>
        public static string GetValueFromSection(string section, string key)
        {
            return GetConfigurationValue<string>(section, key, string.Empty);
        }

        /// <summary>
        /// Returns <code>System.String</code> configuration value from specific section and key, default is 
        /// returned if configuration value is null or empty.
        /// </summary>
        /// <param name="section">Configuration Section Specified in the Configuration File. Should be of type <code>System.Configuration.AppSettingsSection</code> 
        /// that allows appSettings like name value pairs for configuration.</param>
        /// <param name="key">Configuration Key in the specified section.</param>
        /// <param name="defaultValue">Default value returned if the configuration value is null or empty</param>
        /// <returns>String Configuration value from section and key specified, or <param name="defaultValue">defaultValue</param> passed in.</returns>
        public static string GetValueFromSection(string section, string key, string defaultValue)
        {
            return GetConfigurationValue<string>(section, key, defaultValue);
        }

        public static T GetValueFromSection<T>(string key, T defaultValue) where T:IConvertible
        {
            return GetConfigurationValue(DefaultSection, key, defaultValue);
        }

        /// <summary>
        /// Returns configuration value for <param name="key">key</param> from default appSettings 
        /// section of appDomain configuration file.
        /// </summary>
        /// <param name="key">Configuration Key in appSettings section.</param>
        /// <returns>Configuration Value from appSettings Section.</returns>
        public static string GetConfigurationValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Returns typed configuration value for <param name="key">key</param> from default appSettings, 
        /// returns default in case not available.
        /// </summary>
        /// <typeparam name="T">Converts the Configuration Value to this type</typeparam>
        /// <param name="section">Configuration Section Specified in the Configuration File. Should be of type <code>System.Configuration.AppSettingsSection</code> 
        /// that allows appSettings like name value pairs for configuration.</param>
        /// <param name="key">Configuration Key in appSettings section.</param>
        /// <param name="defaultValue">Default value returned if the configuration value is null or empty</param>
        /// <returns>Typed Configuration value from section and key specified, or <param name="defaultValue">defaultValue</param> passed in.</returns>
        public static T GetConfigurationValue<T>(string section, string key, T defaultValue) where T:IConvertible
        {
            if (string.IsNullOrEmpty(section) || string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(section) || string.IsNullOrWhiteSpace(key))
                return defaultValue;

            string configValue = null;
            if (section.Equals(DefaultSection))
                configValue = ConfigurationManager.AppSettings[key];
            else
            {
                var sectionKeys = (NameValueCollection)ConfigurationManager.GetSection(section);
                if (sectionKeys == null || sectionKeys.Count == 0)
                {
                    // TODO: Should empty section result in exception, or should it return defaultValue
                    return defaultValue;
                }
                else
                {
                    configValue = sectionKeys[key];
                    //GetValueFromNameValueCollection(sectionKeys, section, key);
                }
            }

            try
            {
                if (string.IsNullOrEmpty(configValue))
                    return defaultValue;
                else
                {
                    return (T) Convert.ChangeType(configValue, typeof (T));
                }
            }
            catch (System.Exception exception)
            {
                // No action as default value is provided by caller
                return defaultValue;
            }
        }

        /// <summary>
        /// Retrieves a specific Connection String from the configuration file.
        /// </summary>
        /// <param name="connectionStringName">Name of Connectiong String in the configuration file</param>
        /// <returns></returns>
        public static string GetConnectionString(String connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        private static string GetValueFromNameValueCollection(NameValueCollection collection, String section, String key)
        {
            if (collection == null)
                throw new System.ArgumentException(String.Format("Configuration section not found: {0}", section));

            if (collection[key] == null)
                throw new System.ArgumentException(String.Format("Config key not found, Section: {0} | Key: {1}", section, key));

            return collection[key];
        }
    }
}
