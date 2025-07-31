using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Bill
{
    [KnownType("MassiveDataLoadModel")]
    public class MassiveDataLoadModel
    {
        public decimal PaymentsTotal { get; set; } //Amount
        public int BillPayerId { get; set; }       //PayerId
        public int PolicyNumber { get; set; }          //Póliza
        public int EndorsementId { get; set; }     //Endoso
        public int Quota { get; set; }             //Número de cuota
        public int PolicyPayerId { get; set; }     // Pagador de la póliza
        public int PaymentMethodId { get; set; }   // Tipo de pago        
        public int PrefixId { get; set; }   // ramo
        public int CurrencyId { get; set; } // moneda
        public int AgentId { get; set; }//codigo agente poliza
        public string AgentName { get; set; }//nombre agente poliza
        public string BeneficiaryName { get; set; }//nombre beneficiato        
        public string BeneficiaryDocumentNumber { get; set; }//docuymento
        public int EndorsementNumber { get; set; }//numero de endoso
        public int BranchId { get; set; }// sucursal
        public decimal ExchangeRate { get; set; }// Tasa de cambio
    }
}