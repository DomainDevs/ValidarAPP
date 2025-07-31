using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation
{
    [KnownType("PreLiquidationModel")]
    public class PreLiquidationModel
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; }
        public int ImputationId { get; set; }
        public int IsTemporal { get; set; }
        public int IndividualId { get; set; }  //Abonador
        public int PersonTypeId { get; set; }
        public DateTime RegisterDate { get; set; }
        public int SalePointId { get; set; }
        public int StatusId { get; set; }
        public string BranchDescription { get; set; }
        public string CompanyDescription { get; set; }
        public string PayerDocumentNumber { get; set; }  //Abonador
        public string PayerName { get; set; }  //Abonador
        public int TempImputationId { get; set; } 
    }
}