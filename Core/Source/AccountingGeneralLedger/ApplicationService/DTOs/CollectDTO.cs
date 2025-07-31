using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class CollectDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// IsTemporal: Se trata de un recibo temporal?
        /// </summary>        
        [DataMember]
        public bool IsTemporal { get; set; }

        /// <summary>
        /// Date: Fecha del pago
        /// </summary>        
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Description: Descripción del pago
        /// </summary>        
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Comments: Observaciones del pago
        /// </summary>        
        [DataMember]
        public string Comments { get; set; }

        /// <summary>
        /// Branch: Sucursal del pago
        /// </summary>        
        [DataMember]
        public BranchDTO Branch { get; set; }

        /// <summary>
        /// Concept: Concepto de Ingreso
        /// </summary>        
        [DataMember]
        public CollectConceptDTO Concept { get; set; }


        /// <summary>
        /// Payments: Pagos efectuados a la factura
        /// </summary>        
        [DataMember]
        public List<PaymentDTO> Payments { get; set; }

        /// <summary>
        /// PaymentsTotal: Total de todos los pagos
        /// </summary>        
        [DataMember]
        public AmountDTO PaymentsTotal { get; set; }

        /// <summary>
        /// Payer: Abonador 
        /// </summary>        
        [DataMember]
        public PersonDTO Payer { get; set; }

        /// <summary>
        /// Status: Estado de Recibo  
        /// </summary>        
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// User: Usuario que afecta al recibo
        /// </summary>        
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Number: número de caratula 
        /// </summary>        
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// CollectType: Tipo de Ingreso
        /// </summary>        
        [DataMember]
        public int CollectType { get; set; }

        /// <summary>
        /// AccountingCompany: Compañia que genera el pago 
        /// </summary>        
        [DataMember]
        public CompanyDTO AccountingCompany { get; set; }
        [DataMember]
        public int CompanyIndividualId { get; set; }

        /// <summary>
        /// Transaction: Transaccion
        /// </summary>        
        [DataMember]
        public TransactionDTO Transaction { get; set; }

        /// <summary>
        /// PersonType: Tipo de Persona
        /// </summary>        
        [DataMember]
        public PersonTypeDTO PersonType { get; set; }


        /// <summary>
        /// CollectControlId: Controlador cierre de caja
        /// </summary>        
        [DataMember]

        public int CollectControlId { get; set; }

        /// <summary>
        /// SourcePaymentId
        /// </summary>        
        [DataMember]
        public int SourcePaymentId { get; set; }
    }
}
