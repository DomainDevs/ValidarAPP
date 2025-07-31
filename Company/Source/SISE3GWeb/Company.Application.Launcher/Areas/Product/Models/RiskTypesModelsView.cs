using Sistran.Company.Application.ModelServices.Enums;
using System.Collections.Generic;
using MOS = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class RiskTypesModelsView
    {
        /// <summary>
        /// Obtiene o establece el Codigo del tipo de riesgo
        /// </summary>     
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad maxima de riegos permitida
        /// </summary>     
        public int MaxRiskQuantity { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripcion del tipo de riesgo
        /// </summary>     
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Lista Grupo Coberturs
        /// </summary>
        /// <value>
        /// grupo de Coberturas
        /// </value>
        public List<ProductGroupCoverageModelsView> GroupCoverages { get; set; }

        /// <summary>
        /// Gets or sets the pre rule set identifier.
        /// </summary>
        /// <value>
        /// The pre rule set identifier.
        /// </value>
        public int? PreRuleSetId { get; set; }


        /// <summary>
        /// Gets or sets the rule set identifier.
        /// </summary>
        /// <value>
        /// The rule set identifier.
        /// </value>
        public int? RuleSetId { get; set; }

        /// <summary>
        /// Gets or sets the script identifier.
        /// </summary>
        /// <value>
        /// The script identifier.
        /// </value>
        public int? ScriptId { get; set; }

        /// <summary>
        /// Estado del objeto
        /// </summary>        
        public StatusTypeService StatusTypeService { get; set; }

    }
}