#region Using
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Application.TempCommonServices.Models;
#endregion

namespace Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes
{
    /// <summary>
    /// Tipo de asociación de lineas: Por Poliza -> Ramo Tec. -> Subramo Tec.
    /// </summary>
    [DataContract]
    public class ByPolicyLineBusinessSubLineBusiness : LineAssociationType
    {
            /// <summary>
            /// Póliza
            /// </summary>
            [DataMember]
            public Policy Policy { get; set; }

       
            /// <summary>
            /// Ramo Tecnico
            /// </summary>
             [DataMember]
            public LineBusiness LineBusiness { get; set; }

       
            /// <summary>
            /// Subramo Tecnico
            /// </summary>
             [DataMember]
            public SubLineBusiness SubLineBusiness { get; set; }

    }
}