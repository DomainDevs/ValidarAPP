using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ExternalProxyServicesEEProvider.Services
{
    public class UrlApiConstants
    {
        public struct UrlApi
        {
            public readonly static string GetPolicyPayment = "PaymentPolicy2G/GetPolicyPayment";
            public readonly static string GetPaymentsPolicyByDocuments = "PaymentPolicy2G/GetPaymentsPolicyByDocument";
            public readonly static string GetReinsurancePolicy = "ReinsurancePolicy2G/GetReinsurancePolicy";
        }
    }
}
