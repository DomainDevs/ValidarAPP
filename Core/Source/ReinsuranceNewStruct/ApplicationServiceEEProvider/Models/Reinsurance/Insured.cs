using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class Insured
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Agency Agency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public InsuredDeclinedType DeclinedType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public InsuredConcept Concept { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public InsuredProfile Profile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public InsuredSegment Segment { get; set; }

        [DataMember]
        public string IdentificationDocument { get; set; }

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
        public DateTime? DeclinedDate { get; set; }

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
