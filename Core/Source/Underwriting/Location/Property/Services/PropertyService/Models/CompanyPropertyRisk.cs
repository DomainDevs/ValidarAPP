using System;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Location.PropertyServices.Models
{
    /// <summary>
    /// Riesgo hogar
    /// </summary>
    [DataContract]
    public class CompanyPropertyRisk : Sistran.Core.Application.Location.PropertyServices.Models.PropertyRisk
    {
      
        [DataMember]
        public CompanyRisk CompanyRisk { get; set; }


    }
}
