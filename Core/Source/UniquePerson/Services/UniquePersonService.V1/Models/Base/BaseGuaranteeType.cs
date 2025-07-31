using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseGuaranteeType : BaseGeneric
    {

        /// <summary>
        /// IsDocument
        /// </summary>
        [DataMember]
        public bool IsDocument { get; set; }

        /// <summary>
        /// IsOthers
        /// </summary>
        [DataMember]
        public bool IsOthers { get; set; }

        /// <summary>
        /// IsPromissory
        /// </summary>
        [DataMember]
        public bool IsPromissoryNote { get; set; }

        /// <summary>
        /// IsVehicle
        /// </summary>
        [DataMember]
        public bool IsVehicle { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [DataMember]
        public int GuaranteeTypeId { get; set; }

    }
}
