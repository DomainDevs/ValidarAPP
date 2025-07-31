using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// EPIType
    /// </summary>
    [DataContract]
    public class ReportTypeDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripci�n
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Habilitado
        /// </summary>
        [DataMember]
        public bool Enable { get; set; }

    }
}