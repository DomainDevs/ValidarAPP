using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseIndividual:Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public string FullName { get; set; }
        
        /// <summary>
        /// Tipo de individuo
        /// </summary>
        [DataMember]
        public IndividualType IndividualType { get; set; }


        /// <summary>
        /// Tipo Cliente
        /// </summary>
        [DataMember]
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// Tipo Cliente
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// CustomerTypeDescription
        /// </summary>
        [DataMember]
        public string CustomerTypeDescription { get; set; }
        
    }
}
