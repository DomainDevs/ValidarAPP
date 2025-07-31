using System.Collections.Generic;
using System.Runtime.Serialization;



namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class ApplicationParameterDTO 
    {
        [DataMember]
        public int ImputationTypeId { get; set; }

        [DataMember]
        public int SourceId { get; set; }

        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public int BusinessTypeId { get; set; }

        [DataMember]
        public int ComponentId { get; set; }

        [DataMember]
        public int ComponentCollectionId { get; set; }

        [DataMember]
        public decimal IncomeAmount { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// Suma de todos los componentes.
        /// </summary>
        [DataMember]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Id de Pagador
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Campo usado para cuando se realiza la carga de datos por línea y sublínea
        /// </summary>
        [DataMember]
        public List<PrefixComponentCollectionDTO> ComponentCollection{ get; set; }

    }



}
