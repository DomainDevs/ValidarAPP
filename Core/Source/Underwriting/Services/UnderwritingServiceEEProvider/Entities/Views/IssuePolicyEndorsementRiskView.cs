using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable]
    public class IssuePolicyEndorsementRiskView : BusinessView
    {
        /// <summary>
        /// Lista de objetos de la entidad Policy.
        /// </summary>
        /// <value>
        /// Objetos de la entidad Policy.
        /// </value>
        public BusinessCollection PolicyList
        {
            get
            {
                return this["Policy"];
            }
        }
        /// <summary>
        /// Lista de objetos de la entidad EndorsementRisk.
        /// </summary>
        /// <value>
        /// Objetos de la entidad EndorsementRisk.
        /// </value>
        public BusinessCollection EndorsementRiskList
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }
    }
}
