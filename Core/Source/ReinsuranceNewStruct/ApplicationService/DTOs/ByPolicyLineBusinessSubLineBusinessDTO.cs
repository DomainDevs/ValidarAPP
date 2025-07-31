using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Tipo de asociación de lineas: Por Poliza -> Ramo Tec. -> Subramo Tec.
    /// </summary>
    [DataContract]
    public class ByPolicyLineBusinessSubLineBusinessDTO : LineAssociationTypeDTO
    {
        /// <summary>
        /// Póliza
        /// </summary>
        [DataMember]
        public PolicyDTO Policy { get; set; }


        /// <summary>
        /// Ramo Tecnico
        /// </summary>
        [DataMember]
        public LineBusinessDTO LineBusiness { get; set; }

        /// <summary>
        /// Subramo Tecnico
        /// </summary>
        [DataMember]
        public SubLineBusinessDTO SubLineBusiness { get; set; }
    }
}