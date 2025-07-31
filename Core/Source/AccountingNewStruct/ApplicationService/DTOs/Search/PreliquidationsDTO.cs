using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class PreliquidationsDTO 
    {
        [DataMember]
        public int PreliquidationId { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string StatusDescription { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string BranchDescription { get; set; }
        [DataMember]
        public int UsertId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int SalesPointId { get; set; }
        [DataMember]
        public string SalesPointDescription { get; set; }
        [DataMember]
        public string RegisterDate { get; set; }
        [DataMember]
        public int AccountingCompanyId { get; set; }
        [DataMember]
        public string AccountingCompanyDescription { get; set; }
        [DataMember]
        public int PersonTypeId { get; set; }
        [DataMember]
        public string PersonTypeDescription { get; set; }
        [DataMember]
        public int BeneficiaryIndividualId { get; set; }
        [DataMember]
        public string BeneficiaryDocumentNumber { get; set; }
        [DataMember]
        public string BeneficiaryName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int TempImputationId { get; set; }
        [DataMember]
        public int SourceId { get; set; }
        [DataMember]
        public int ImputationTypeId { get; set; }
        [DataMember]
        public string ImputationTypeDescription { get; set; }
        [DataMember]
        public int IsRealSource { get; set; }
        [DataMember]
        public string StartDate { get; set; } //utilizado para la búsqueda
        [DataMember]
        public string EndDate { get; set; } //utilizado para la búsqueda
        [DataMember]
        public int Rows { get; set; }
        [DataMember]
        public decimal TotalAmount { get; set; }
    }
}
