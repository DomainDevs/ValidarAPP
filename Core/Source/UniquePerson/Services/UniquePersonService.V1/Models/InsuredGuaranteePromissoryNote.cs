using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    [DataContract]
    public class InsuredGuaranteePromissoryNote : BaseInsuredGuarantee
    {
        /// <summary>
        /// Numero de documento
        /// </summary>
        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Valor Nominal
        /// </summary>
        [DataMember]
        public Decimal DocumentValueAmount { get; set; }

        /// <summary>
        /// Fecha constitucion
        /// </summary>
        [DataMember]
        public DateTime ConstitutionDate { get; set; }

        /// <summary>
        /// Fecha de vencimiento
        /// </summary>
        [DataMember]
        public DateTime? ExtDate { get; set; }

        /// <summary>
        /// Tipode pagare
        /// </summary>
        [DataMember]
        public PromissoryNoteType PromissoryNoteType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int SignatoriesNumber { get; set; }   
    }
}
