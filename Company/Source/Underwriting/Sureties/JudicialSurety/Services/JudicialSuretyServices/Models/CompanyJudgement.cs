using Sistran.Company.Application.UnderwritingServices.Models;
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Company.Application.Sureties.Models;

namespace Sistran.Company.Application.Sureties.JudicialSuretyServices.Models
{

    /// <summary>
    /// Riesgo Caución Judicial
    /// </summary>
    [DataContract]
    public class CompanyJudgement : BaseJudgement
    {
      
        public CompanyJudgement()
        {
            Risk = new CompanyRisk();
        }

        /// <summary>
        /// Gets or sets the risk.
        /// </summary>
        /// <value>
        /// The risk.
        /// </value>
        [DataMember]
        public CompanyRisk Risk { get; set; }
        /// <summary>
        /// Gets or sets the article.
        /// </summary>
        /// <value>
        /// The article.
        /// </value>
        [DataMember]
        public CompanyArticle Article { get; set; }

        /// <summary>
        /// Gets or sets the court.
        /// </summary>
        /// <value>
        /// The court.
        /// </value>
        [DataMember]
        public CompanyCourt Court { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        [DataMember]
        public City City { get; set; }

        /// <summary>
        /// Gets or sets the guarantees.
        /// </summary>
        /// <value>
        /// The guarantees.
        /// </value>
        [DataMember]
        public List<CiaRiskSuretyGuarantee> Guarantees { get; set; }

        /// <summary>
        /// Gets or sets the attorney.
        /// </summary>
        /// <value>
        /// The attorney.
        /// </value>
        [DataMember]
        public CompanyAttorney Attorney { get; set; }
    }
}
