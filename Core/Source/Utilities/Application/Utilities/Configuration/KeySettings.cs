using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Sistran.Core.Framework;
namespace Sistran.Core.Application.Utilities.Configuration
{
    using DAOs;

    public class KeySettings
    {
        private const string OnerousBeneficiaryTypeIdKey = "OnerousBeneficiaryTypeId";
        private const string LeasingBeneficiaryTypeIdKey = "LeasingBeneficiaryTypeId";
        private const string NotApplyBeneficiaryTypeIdKey = "NotApplyBeneficiaryTypeId";
        private const string DebugParallelKey = "DebugParallel";
        private const string MaxThreadCollectiveKey = "MaxThreadCollective";
        private const string ServiceResourceCultureKey = "ServiceResourceCulture";
        private const string ReportErrorSeparator = "ReportErrorSeparator";
        private const string MaxRowNotifications = "MaxRowNotifications";
        private const string NotificateQueeque = "NotificateQueeque";


        private static readonly int? _onerousBeneficiaryTypeId = GetIntSetting(OnerousBeneficiaryTypeIdKey);
        private static readonly int? _leasingBeneficiaryTypeId = GetIntSetting(LeasingBeneficiaryTypeIdKey);
        private static readonly int? _notApplyBeneficiaryTypeId = GetIntSetting(NotApplyBeneficiaryTypeIdKey);

        /// <summary>
        /// Gets the rules values.
        /// </summary>
        /// <value>
        /// The rules values.
        /// </value>
        public static List<int> RulesValues { get; private set; }

        /// <summary>
        /// Gets the entity path.
        /// </summary>
        /// <value>
        /// The entity path.
        /// </value>
        public static string EntityPath { get; private set; }

        /// <summary>
        /// Gets the entity path.
        /// </summary>
        /// <value>
        /// The entity path.
        /// </value>
        public static string EntityPathCompany { get; private set; }

        /// <summary>
        /// Gets the entity path base.
        /// </summary>
        /// <value>
        /// The entity path base.
        /// </value>
        public static string EntityPathBase { get; private set; }



        public static bool InitializeCache { get; private set; }

        public static string FullDatePattern { get; private set; }

        public static List<string> ExcludeFolders { get; private set; }
        static KeySettings()
        {
            DebugParallel = GetIntSetting(DebugParallelKey, 1).Value;
            MaxtTask = GetIntSetting(MaxThreadCollectiveKey, Environment.ProcessorCount).Value;

            LoadParametrizationSystemDAO parametrizationSystemDao = new LoadParametrizationSystemDAO();
            RulesValues = new List<int>(parametrizationSystemDao.LoadEnumParameterValuesFromDB(Enum.GetNames(typeof(Enums.FacadeType))));

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["EntityPath"]))
            {
                throw new ArgumentException("no existe la llave {EntityPath}");
            }
            else
            {
                EntityPath = ConfigurationManager.AppSettings["EntityPath"];
            }
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["EntityPathCompany"]))
            {
                throw new ArgumentException("no existe la llave {EntityPathCompany}");
            }
            else
            {
                EntityPathCompany = ConfigurationManager.AppSettings["EntityPathCompany"];
            }

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["EntityPathBase"]))
            {
                throw new ArgumentException("no existe la llave {EntityPathBase}");
            }
            else
            {
                EntityPathBase = ConfigurationManager.AppSettings["EntityPathBase"];
            }

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["InitializeCache"]))
            {
                InitializeCache = true;
            }
            else
            {
                InitializeCache = Convert.ToBoolean(ConfigurationManager.AppSettings["InitializeCache"]);
            }

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["FullDatePattern"]))
            {
                FullDatePattern = "dd/MM/yyyy HH:mm:ss"; //Hora Militar
            }
            else
            {
                FullDatePattern = ConfigurationManager.AppSettings["FullDatePattern"];
            }
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ExcludeFolders"]))
            {
                ExcludeFolders = null;
            }
            else
            {
                ExcludeFolders = ConfigurationManager.AppSettings["ExcludeFolders"].Split(',').ToList();
            }

        }
        public static int DebugParallel { get; }
        public static int MaxtTask { get; }

        public static int OnerousBeneficiaryTypeId
        {
            get
            {
                if (!_onerousBeneficiaryTypeId.HasValue)
                {
                    throw new ValidationException("No se configuró el tipo de beneficiaro oneroso en el AppConfig");
                }
                return _onerousBeneficiaryTypeId.Value;
            }
        }
        public static int LeasingBeneficiaryTypeId
        {
            get
            {
                if (!_leasingBeneficiaryTypeId.HasValue)
                {
                    throw new ValidationException("No se configuró el tipo de beneficiaro no oneroso en el AppConfig");
                }
                return _leasingBeneficiaryTypeId.Value;
            }
        }

        public static int NotApplyBeneficiaryTypeId
        {
            get
            {
                if (!_notApplyBeneficiaryTypeId.HasValue)
                {
                    throw new ValidationException("No se configuró el tipo de beneficiaro no aplica en el AppConfig");
                }
                return _notApplyBeneficiaryTypeId.Value;
            }
        }

        #region Culture
        /// <summary>
        /// Retorna el valor de la cultura configurada en el AppConfig con la llave ServiceResourceCulture, por defecto queda en español (es)
        /// </summary>
        public static CultureInfo ServiceResourceCulture => serviceResourceCulture;

        private static readonly CultureInfo serviceResourceCulture = GetServiceResourceCulture();
        private static CultureInfo GetServiceResourceCulture()
        {
            string culture = ConfigurationManager.AppSettings[ServiceResourceCultureKey];
            if (string.IsNullOrEmpty(culture))
            {
                culture = "es";
            }
            return new CultureInfo(culture);
        }
        #endregion Culture

        private static int? GetIntSetting(string key, int? defaultValue = null)
        {
            string setting = ConfigurationManager.AppSettings[key];
            int value;
            if (string.IsNullOrEmpty(setting) || !int.TryParse(setting, out value))
            {
                return defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Obtener valor de Una Llave
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int GetSetting(string key, int? defaultValue = null)
        {
            string setting = ConfigurationManager.AppSettings[key];
            int value;
            if (string.IsNullOrEmpty(setting) || !int.TryParse(setting, out value))
            {
                return defaultValue == null ? -1 : defaultValue.GetValueOrDefault();
            }
            return value;
        }

        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T GetSetting<T>(string key)
        {
            T t = default(T);
            return GetSetting<T>(key, t);
        }
        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T GetSetting<T>(string key, T defaultValue)
        {
            string setting = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(setting) || defaultValue != null)
            {
                return defaultValue == null ? default(T) : defaultValue;
            }
            return (T)Convert.ChangeType(key, typeof(T));
        }

        /// <summary>
        /// Asigna el separador de errores 
        /// </summary>
        /// <returns></returns>
        public static string ReportErrorSeparatorMessage()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[ReportErrorSeparator]))
            {
                return "";
            }
            return ConfigurationManager.AppSettings[ReportErrorSeparator].ToString();
        }
        /// <summary>
        /// Asigna el maximo de registros de notificaciones a consultar 
        /// </summary>
        /// <returns></returns>
        public static int MaxRowNotificationsSetting()
        {
            string setting = ConfigurationManager.AppSettings[MaxRowNotifications];
            int value;
            if (string.IsNullOrEmpty(setting) || !int.TryParse(setting, out value))
            {
                return 0;
            }
            return value;
        }

        /// <summary>
        /// Asigna el NotificateQueeque
        /// </summary>
        /// <returns></returns>
        public static bool ValidateNotificateQueeque()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[NotificateQueeque]))
            {
                return false;
            }
            return Convert.ToBoolean(ConfigurationManager.AppSettings[NotificateQueeque]);
        }

    }
}
