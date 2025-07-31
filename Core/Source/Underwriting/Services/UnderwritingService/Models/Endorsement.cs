using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Endoso
    /// </summary>
    [DataContract]
    public class Endorsement : BaseEndorsement
    {
        /// <summary>
        /// Textos
        /// </summary>
        [DataMember]
        public Text Text { get; set; }
        /// <summary>
        /// Ramo
        /// </summary>
        [DataMember]
        public BasePrefix Prefix { get; set; }
        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public BaseBranch Branch { get; set; }

        [DataMember]
        public RiskChangeText Risk { get; set; }


    }
}
