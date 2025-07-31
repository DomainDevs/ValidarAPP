using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Utilities.Configuration
{
    public static class Settings
    {

        private const string UseReplicatedDataBase = "UseReplicatedDatabase";
        private const string ImplementWebServiceS = "ImplementWebServices";
        private const string LinkServerName = "LinkServerName";
        private const string LinkDatabaseName = "LinkDatabaseName";
        private const string ImplementValidate2GName = "ImplementValidate2GName";
        private const string ReportErrorSeparator = "ReportErrorSeparator";

        /// <summary>
        /// Si usa base de datos de repplica
        /// </summary>
        /// <returns></returns>
        public static bool UseReplicatedDatabase()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[UseReplicatedDataBase]))
            {
                return false;
            }
            return Convert.ToBoolean(ConfigurationManager.AppSettings[UseReplicatedDataBase].ToString());
        }

        /// <summary>
        /// Si consulta externos para terceros
        /// </summary>
        /// <returns></returns>
        public static bool ImplementWebServices()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[ImplementWebServiceS]))
            {
                return false;
            }
            return Convert.ToBoolean(ConfigurationManager.AppSettings[ImplementWebServiceS].ToString());
        }

        /// <summary>
        /// Link server para hacer la conexion
        /// </summary>
        /// <returns></returns>
        public static string LinkServer()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[LinkServerName]))
            {
                return "";
            }
            return ConfigurationManager.AppSettings[LinkServerName];
        }

        /// <summary>
        /// Nombre de base de datos de replica
        /// </summary>
        /// <returns></returns>
        public static string LinkDatabase()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[LinkDatabaseName]))
            {
                return "";
            }
            return ConfigurationManager.AppSettings[LinkDatabaseName];
        }

        /// <summary>
        /// Si usa base de datos SyBase de 2G
        /// </summary>
        /// <returns></returns>
        public static bool ImplementValidate2G()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[ImplementValidate2GName]))
            {
                return false;
            }
            return Convert.ToBoolean(ConfigurationManager.AppSettings[ImplementValidate2GName].ToString());
        }
    }
}
