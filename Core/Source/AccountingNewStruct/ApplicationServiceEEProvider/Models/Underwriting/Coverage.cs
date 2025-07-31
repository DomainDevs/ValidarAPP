using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base;
using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting
{
    /// <summary>
    /// Coberturas
    /// </summary>
    [DataContract]
    public class Coverage : BaseCoverage
    {
        /// <summary>
        /// Ramo tecnico
        /// </summary>
        [DataMember]
        public SubLineBusiness SubLineBusiness { get; set; }

        /// <summary>
        /// objeto del seguro
        /// </summary>
        [DataMember]
        public InsuredObject InsuredObject { get; set; }

        /// <summary>
        /// Gets or sets las coberturas aliadas
        /// </summary>        
        [DataMember]
        public List<Coverage> CoverageAllied { get; set; }

        /// <summary>
        /// Gets or sets las coberturas aliadas
        /// </summary>        
        [DataMember]
        public int? AllyCoverageId { get; set; }
        /// <summary>

    }
}
