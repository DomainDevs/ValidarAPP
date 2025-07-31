using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class InsuredConcept
    {
        /// <summary>
        /// OObtiene o establece el codigo de asegurado
        /// </summary>
        /// <value>
        /// The insured code.
        /// </value>
        [DataMember]
        public int InsuredCode { get; set; }

        /// <summary>
        /// Gets or sets the is holder.
        /// </summary>
        /// <value>
        /// The is holder.
        /// </value>
        [DataMember]
        public bool? IsInsured { get; set; }

        /// <summary>
        /// Gets or sets the is holder.
        /// </summary>
        /// <value>
        /// The is holder.
        /// </value>
        [DataMember]
        public bool? IsHolder { get; set; }

        /// <summary>
        /// Obtiene o establece is beneficiary.
        /// </summary>
        /// <value>
        /// The is beneficiary.
        /// </value>
        [DataMember]
        public bool? IsBeneficiary { get; set; }

        /// <summary>
        /// Obtiene o establece the is representative.
        /// </summary>
        /// <value>
        /// The is representative.
        /// </value>
        [DataMember]
        public bool? IsRepresentative { get; set; }

        /// <summary>
        ///Obtiene o establece the is surety.
        /// </summary>
        /// <value>
        /// The is surety.
        /// </value>
        [DataMember]
        public bool? IsSurety { get; set; }

        /// <summary>
        /// Obtiene o establece the is consortium.
        /// </summary>
        /// <value>
        /// The is consortium.
        /// </value>
        [DataMember]
        public bool? IsConsortium { get; set; }

        /// <summary>
        /// Obtiene o establece the is payer.
        /// </summary>
        /// <value>
        /// Si se le asigna en concepto de pagador
        /// </value>
        [DataMember]
        public bool? IsPayer { get; set; }
    }
}
