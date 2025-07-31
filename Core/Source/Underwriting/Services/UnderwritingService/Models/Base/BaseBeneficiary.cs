using Sistran.Core.Application.Extensions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Beneficiario base
    /// </summary>
    [DataContract]
    public class BaseBeneficiary : Extension
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Participation
        /// </summary>     
        [DataMember]
        public decimal Participation { get; set; }

        /// <summary>
        /// Descripcion Tipo del Beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryTypeDescription { get; set; }      

        /// <summary>
        /// Tipo de cliente
        /// </summary>
        [DataMember]
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// Tipo de persona
        /// </summary>
        [DataMember]
        public IndividualType IndividualType { get; set; }

        [DataMember]
        public int CodeBeneficiary { get; set; }

    }
}
