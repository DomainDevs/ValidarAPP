using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Models
{
    public class ContextModel
    {
        /// <summary>
        /// Ramo
        /// </summary>
        public SubCoveredRiskType? SubCoveredRiskType { get; set; }

        /// <summary>
        /// Tipo de endoso
        /// </summary>
        public EndorsementType? EndorsementType { get; set; }

        /// <summary>
        /// Tipo de operación
        /// </summary>
        public TemporalType? TemporalType { get; set; }

        public ContextModel(SubCoveredRiskType? SubCoveredRiskType)
        {
            this.SubCoveredRiskType = SubCoveredRiskType;
        }

        public ContextModel(TemporalType? TemporalType)
        {
            this.TemporalType = TemporalType;
        }

        public ContextModel(SubCoveredRiskType? SubCoveredRiskType, TemporalType? TemporalType)
        {
            this.SubCoveredRiskType = SubCoveredRiskType;
            this.TemporalType = TemporalType;
        }

        public ContextModel(SubCoveredRiskType? SubCoveredRiskType, TemporalType? TemporalType, EndorsementType? EndorsementType)
        {
            this.SubCoveredRiskType = SubCoveredRiskType;
            this.EndorsementType = EndorsementType;
            this.TemporalType = TemporalType;
        }
    }
}