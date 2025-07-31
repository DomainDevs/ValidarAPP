using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseCompany : BaseIndividual
    {
        /// <summary>
        /// CountryId
        /// </summary>
        [DataMember]
        public int CountryId { get; set; }

        /// <summary>
        /// VerifyDigit
        /// </summary>
        [DataMember]
        public int? VerifyDigit { get; set; }
    }
}
