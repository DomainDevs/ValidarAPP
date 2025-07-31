using System;
using System.Configuration;
using Utiles.Extentions;

namespace Utiles.Readers
{
    public class ConfigurationReadAsistance
    {
        #region METODOS

        public static T GetConfigurationValue<T>(string configurationKeyName)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var element = appSettings[configurationKeyName];
                if (element != null)
                {
                    return (T)Convert.ChangeType(element, typeof(T));
                }
                else
                {
                    throw new Exception(string.Format("La llave de configuración denominada {0} no se encuentra en el archivo", configurationKeyName));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T GetConfigurationValue<T>(string sectionLikeJson, string configurationKeyName)
        {
            var element = Newtonsoft.Json.Linq.JObject.Parse(sectionLikeJson)[configurationKeyName];

            if (element == null)
            {
                throw new Exception(string.Format("La llave de configuración denominada {0} no se encuentra en el archivo", configurationKeyName));
            }
            return (T)Convert.ChangeType(element, typeof(T));
        }

        public static string GetCustomSectionLikeJson(string customSection)
        {
            var connectionManagerDatabaseServers = ConfigurationManager.GetSection(customSection);

            if (connectionManagerDatabaseServers == null)
            {
                throw new Exception(string.Format("La sección de configuración denominada {0} no se encuentra en el archivo", customSection));
            }

            var jsonSection = connectionManagerDatabaseServers.GetJson();

            return jsonSection;
        }

        public static T GetConfigurationValue<T>(string configurationKeyName, T defaultValue)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var element = appSettings[configurationKeyName];
                if (element != null)
                {
                    return (T)Convert.ChangeType(element, typeof(T));
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion METODOS
    }
}