using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseBank : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Banco
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int? BankTypeCode { get; set; }
        [DataMember]
        public int? Aba { get; set; }
        [DataMember]
        public string Swift { get; set; }
        [DataMember]
        public string ChargeAchId { get; set; }
        [DataMember]
        public string PaymentAchId { get; set; }
        [DataMember]
        public bool? ChargeAch { get; set; }
        [DataMember]
        public bool? PaymentAch { get; set; }
        [DataMember]
        public bool? Enable { get; set; }
        [DataMember]
        public string ChisId { get; set; }
        [DataMember]
        public string TributaryIdNo { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public int? SuperCode { get; set; }
        [DataMember]
        public int BankCode { get; set; }
        [DataMember]
        public int? CountryCode { get; set; }
        [DataMember]
        public int? SquareCode { get; set; }
        [DataMember]
        public string CityDescription { get; set; }
        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public int? BranchOffice { get; set; }
    }
}
