using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseInsured : BaseIndividual
    {
        /// <summary>
        /// Gets or sets the insured identifier.
        /// </summary>
        /// <value>
        /// The insured identifier.
        /// </value>
        [DataMember]
        public int InsuredCode { get; set; }

        /// <summary>
        /// Gets or sets the insured identifier.
        /// </summary>
        /// <value>
        /// decline date.
        /// </value>
        [DataMember]
        public DateTime? DeclinedDate{ get; set; }

        /// <summary>
        /// Gets or sets the insured identifier.
        /// </summary>
        /// <value>
        /// EnteredDate.
        /// </value>
        [DataMember]
        public DateTime EnteredDate { get; set; }

        /// <summary>
        /// Gets or sets the insured identifier.
        /// </summary>
        /// <value>
        /// EnteredDate.
        /// </value>
        [DataMember]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// Gets or sets the insured identifier.
        /// </summary>
        /// <value>
        /// EnteredDate.
        /// </value>
        [DataMember]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Annotations.
        /// </summary>
        /// <value>
        /// Annotations.
        /// </value>
        [DataMember]
        public string Annotations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsSMS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsMailAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ReferedBy { get; set; }

        /// <summary>
        ///segundo  Apellido
        /// </summary>
        [DataMember]
        public string SecondSurName { get; set; }


    }
}
