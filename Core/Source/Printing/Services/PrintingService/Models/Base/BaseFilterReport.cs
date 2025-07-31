using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.PrintingServices.Models.Base
{
    [DataContract]
    public class BaseFilterReport : Extension
    {
        /// <summary>
        /// Id Temporal
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// Id Endoso
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }
    }
}
