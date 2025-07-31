using Sistran.Core.Application.UniqueUserServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Models
{
    [DataContract]
    public class IndividualRelationApp : BaseIndividualRelationApp
    {
        /// <summary>
        /// Agencia
        /// </summary>
        [DataMember]
        public UserAgency Agency { get; set; }
    }
}