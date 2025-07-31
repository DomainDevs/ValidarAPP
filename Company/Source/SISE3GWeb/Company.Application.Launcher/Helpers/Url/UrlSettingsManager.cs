using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public static class UrlSettingsManager
    {
        #region constants
        private static string urlsSection = "urlAddresses";
        #endregion
        #region members
        private static UrlsSection _settings = ConfigurationManager.GetSection(urlsSection) as UrlsSection;
        #endregion
        #region Properties
        public static UrlsSection Settings
        {
            get { return _settings; }
        }
        #endregion
    }
}