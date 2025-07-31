using System;
using System.Collections.Generic;

//Sistran
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Models
{
    /// <summary>
    /// Collect: Ingreso de Caja
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class Collect
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
        public Branch Branch { get; set; }

        /// <summary>
        /// Concept: Concepto de Ingreso
        /// </summary>        
        [DataMember]
        public CollectConcept Concept { get; set; }


        /// <summary>
        /// Payments: Pagos efectuados a la factura
        /// </summary>        
        [DataMember]
        public List<Payment> Payments { get; set; }

        /// <summary>
        /// PaymentsTotal: Total de todos los pagos
        /// </summary>        
        [DataMember]
        public Amount PaymentsTotal { get; set; }

        /// <summary>
        /// Payer: Abonador 
        /// </summary>        
        [DataMember]
        public Person Payer { get; set; }

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
        public CollectTypes CollectType { get; set; }


        /// <summary>
        /// AccountingCompany: Compañia que genera el pago 
        /// </summary>        
        [DataMember]
        public Company AccountingCompany { get; set; }

        /// <summary>
        /// Transaction: Transaccion
        /// </summary>        
        [DataMember]
        public Transaction Transaction { get; set; }

        /// <summary>
        /// PersonType: Tipo de Persona
        /// </summary>        
        [DataMember]
        public PersonType PersonType { get; set; }

        /// <summary>
        /// CollectControlId: Controlador cierre de caja
        /// </summary>        
        [DataMember]

        public int CollectControlId { get; set; }

        public Collect()
        {
            this.Payments = new List<Payment>();
            this.Date = DateTime.Now;
        }
    }
}
