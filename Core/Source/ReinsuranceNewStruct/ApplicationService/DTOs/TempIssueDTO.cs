using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class TempIssueDTO
    {
        /// <summary>
		/// Atributo para la propiedad TempReinsuranceProcessCode.
		/// </summary>
        [DataMember] 
        public int ProcessId {get; set;}
        /// <summary>
        /// Atributo para la propiedad EndorsementCode.
        /// </summary>
        [DataMember] 
        public int EndorsementId {get; set;}
        /// <summary>
        /// Atributo para la propiedad ReinsuranceNumber.
        /// </summary>
        [DataMember] 
        public int ReinsuranceNumber {get; set;}
        /// <summary>
        /// Atributo para la propiedad BranchCode.
        /// </summary>
        [DataMember] 
        public int BranchId {get; set;}
        /// <summary>
        /// Atributo para la propiedad PrefixCode.
        /// </summary>
        [DataMember]
        public int PrefixId {get; set;}
        /// <summary>
        /// Atributo para la propiedad CurrencyCode.
        /// </summary>
        [DataMember]
        public int CurrencyId {get; set;}
        /// <summary>
        /// Atributo para la propiedad DocumentNum.
        /// </summary>
        [DataMember] 
        public decimal DocumentNum {get; set;}
        /// <summary>
        /// Atributo para la propiedad PolicyId.
        /// </summary>
        [DataMember]
        public int PolicyId  {get; set;}
        /// <summary>
        /// Atributo para la propiedad EndorsementNumber.
        /// </summary>
        [DataMember] 
        public decimal EndorsementNumber {get; set;}
        /// <summary>
        /// Atributo para la propiedad BusinessTypeCode.
        /// </summary>
        [DataMember] 
        public int BusinessTypeId {get; set;}
        /// <summary>
        /// Atributo para la propiedad InsuredCode.
        /// </summary>
        [DataMember] 
        public int InsuredId {get; set;}
        /// <summary>
        /// Atributo para la propiedad PolicyIssueDate.
        /// </summary>
        [DataMember] 
        public DateTime PolicyIssueDate {get; set;}
        /// <summary>
        /// Atributo para la propiedad EndorsementIssueDate.
        /// </summary>
        [DataMember] 
        public DateTime EndorsementIssueDate {get; set;}
        /// <summary>
        /// Atributo para la propiedad PolicyCurrentFrom.
        /// </summary>
        [DataMember] 
        public DateTime PolicyCurrentFrom {get; set;}
        /// <summary>
        /// Atributo para la propiedad PolicyCurrentTo.
        /// </summary>
        [DataMember] 
        public DateTime PolicyCurrentTo {get; set;}
        /// <summary>
        /// Atributo para la propiedad EndorsementCurrentFrom.
        /// </summary>
        [DataMember] 
        public DateTime EndorsementCurrentFrom {get; set;}
        /// <summary>
        /// Atributo para la propiedad EndorsementCurrentTo.
        /// </summary>
        [DataMember] 
        public DateTime EndorsementCurrentTo {get; set;}
        /// <summary>
        /// Atributo para la propiedad ProductId.
        /// </summary>
        [DataMember] 
        public int? ProductId  {get; set;}
    }
}
