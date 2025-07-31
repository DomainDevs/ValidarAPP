using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseIndividualRole : Extension
    {
        /// <summary>
        /// IndividualId
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// RoleId
        /// </summary>
        [DataMember]
        public int RoleId { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
