using System;
using System.Runtime.Serialization;



namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class PremiumReceivableSearchPolicyDTO 
    {
        //campo compuestos
        [DataMember]
        public string BranchPrefixPolicyEndorsement { get; set; } //sucursal, ramo, poliza
        [DataMember]
        public string InsuredDocumentNumberName { get; set; } //documento, nombre de asegurado
        [DataMember]
        public string PayerDocumentNumberName { get; set; } //documento, nombre de pagador
        [DataMember]
        public string PolicyAgentDocumentNumberName { get; set; } //documento, nombre de agente


        //campos individuales

        //datos de Póliza
        [DataMember]
        public string PolicyDocumentNumber { get; set; }
        [DataMember]
        public int PolicyId { get; set; }

        //Datos de endoso
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public string EndorsementDocumentNumber { get; set; }
        [DataMember]
        public int EndorsementTypeId { get; set; }
        [DataMember]
        public string EndorsementTypeDescription { get; set; }

        //datos de pagador
        [DataMember]
        public int PayerIndividualId { get; set; } //individualId
        [DataMember]
        public int PayerId { get; set; } //personId
        [DataMember]
        public string PayerDocumentNumber { get; set; }
        [DataMember]
        public string PayerName { get; set; }
        [DataMember]
        public string Address { get; set; }

        //numero de cuota
        [DataMember]
        public int PaymentNumber { get; set; }

        //datos de agrupación de póliza
        [DataMember]
        public int CollectGroupId { get; set; }
        [DataMember]
        public string CollectGroupDescription { get; set; }

        //detalles de pago de póliza
        [DataMember]
        public DateTime PaymentExpirationDate { get; set; }
        [DataMember]
        public decimal Amount { get; set; } //valor de la cuota
        [DataMember]
        public decimal PaidAmount { get; set; } //saldo pagado
        [DataMember]
        public decimal PaymentAmount { get; set; } //saldo pendiente a pagar.
        [DataMember]
        public decimal TotalPremium { get; set; } //Prima Total.
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }

        //tipo de negocio, sucursal y ramo
        [DataMember]
        public int BussinessTypeId { get; set; }
        [DataMember]
        public string BussinessTypeDescription { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string BranchDescription { get; set; }
        [DataMember]
        public int PrefixId { get; set; }
        [DataMember]
        public string PrefixDescription { get; set; }
        [DataMember]
        public string PrefixTyniDescription { get; set; }

        //datos de Agente
        [DataMember]
        public int PolicyAgentId { get; set; } //individualId
        [DataMember]
        public string PolicyAgentDocumentNumber { get; set; }
        [DataMember]
        public string PolicyAgentName { get; set; }

        //datos de asegurado
        [DataMember]
        public int InsuredIndividualId { get; set; } //individualId
        [DataMember]
        public int InsuredId { get; set; } //personId
        [DataMember]
        public string InsuredDocumentNumber { get; set; }
        [DataMember]
        public string InsuredName { get; set; }

        //valores de la póliza
        [DataMember]
        public decimal AgentParticipationPercentage { get; set; }
        [DataMember]
        public decimal StandarCommissionPercentage { get; set; }
        [DataMember]
        public decimal NetPremium { get; set; }
        [DataMember]
        public decimal PendingCommission { get; set; }
        [DataMember]
        public decimal DiscountedCommission { get; set; }
        [DataMember]
        public decimal ExcessPayment { get; set; }

        //datos para recuotificación
        [DataMember]
        public decimal Expenses { get; set; }

        [DataMember]
        public decimal Surcharges { get; set; }

        [DataMember]
        public decimal Taxes { get; set; }

        [DataMember]
        public decimal Discounts { get; set; }

        [DataMember]
        public DateTime PolicyIssuanceDate { get; set; }

        [DataMember]
        public DateTime PolicyCurrentFrom { get; set; }

        [DataMember]
        public DateTime PolicyCurrentTo { get; set; }

        [DataMember]
        public int PaymentMethodId { get; set; }

        [DataMember]
        public string PaymentMethodDescription { get; set; }

        [DataMember]
        public int PaymentScheduleId { get; set; }
        [DataMember]
        public string PaymentScheduleDescription { get; set; }

        //Se agrego este campo para que al añadir items a pagar respete el orden de cuota
        [DataMember]
        public string ItemId { get; set; }
        
        //indicador de filas para paginación.
        [DataMember]
        public int Rows { get; set; }

        //Observaciones del pago realizado
        [DataMember]
        public string Observations { get; set; }
        //Impuesto IVA
        [DataMember]
        public string Tax { get; set; }
        //identificador del componente
        [DataMember]
        public string ComponentId { get; set; }
    }
}
