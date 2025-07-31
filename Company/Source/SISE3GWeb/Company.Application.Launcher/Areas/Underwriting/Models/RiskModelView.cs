using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class RiskModelView
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public CompanyText Text { get; set; }

        /// <summary>
        /// Gets or sets the clauses.
        /// </summary>
        /// <value>
        /// The clauses.
        /// </value>
        public List<CompanyClause> Clauses { get; set; }

        /// <summary>
        /// Gets or sets the beneficiaries.
        /// </summary>
        /// <value>
        /// The beneficiaries.
        /// </value>
        public List<CompanyBeneficiary> Beneficiaries { get; set; }
    }
}