using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Sistran.Company.Application.ExternalProxyServicesEEProvider.Services
{
    

    public class ReinsurancePolicyServices
    {
        private string _uriApiSettingModel = "";
        private bool _hassProxy = false;
        private string _proxy = "";
        private int _proxyPort = 0;
        private TimeSpan _timeoutSeconds;

        public ReinsurancePolicyServices()
        {
            _uriApiSettingModel = WebConfigurationManager.AppSettings["UriApiPayment"];
            _hassProxy = Convert.ToBoolean(WebConfigurationManager.AppSettings["UriHasProxy"]);
            _proxy = WebConfigurationManager.AppSettings["UriProxy"];
            _proxyPort = Int32.Parse(WebConfigurationManager.AppSettings["UriProxyPort"]);
        }

        public string GetReinsurancePolicy(string request)
        {
            TimeSpan timeoutSeconds = (_timeoutSeconds != null || _timeoutSeconds != TimeSpan.Zero) ? _timeoutSeconds : TimeSpan.Parse("00:00:05");

            _uriApiSettingModel += UrlApiConstants.UrlApi.GetReinsurancePolicy;
            return ClientServices.ExecuteRequest(_hassProxy, _proxy, _proxyPort ,_uriApiSettingModel, request, timeoutSeconds);
        }
    }
}