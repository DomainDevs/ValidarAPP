//Sistran
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReconciliationServices.Models
{
    /// <summary>
    /// Statement: Extracto 
    /// </summary>
    [DataContract]
    public class Statement 
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// StatementType: Tipo de Extracto( Banco, Contabilidad) 
        /// </summary>        
        [DataMember]
        public StatementTypes StatementType { get; set; }

        /// <summary>
        /// BankAccountCompany: Cuenta Bancaria de la Compañia
        /// </summary>
        [DataMember]
        public BankAccountCompanyDTO BankAccountCompany { get; set; }
        
        /// <summary>
        /// Branch: Sucursal
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// Date:Fecha
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// ProcessDate:Fecha de Proceso de Carga
        /// </summary>
        [DataMember]
        public DateTime ProcessDate { get; set; }

        /// <summary>
        /// ProcessNumber : Numero de Proceso de Carga
        /// </summary>        
        [DataMember]
        public int ProcessNumber { get; set; }

        /// <summary>
        /// ReconciliationMovementType: Tipo de Movimiento de Conciliacion
        /// </summary>
        [DataMember]
        public ReconciliationMovementTypeDTO ReconciliationMovementType { get; set; }

        /// <summary>
        /// DocumentNumber: Numero de Documento
        /// </summary>
        [DataMember]
        public string DocumentNumber { get; set; }
        
        /// <summary>
        /// ThirdPerson: Tercero
        /// </summary>
        [DataMember]
        public Individual ThirdPerson { get; set; }

        /// <summary>
        /// Description: Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Amount: Importe
        /// </summary>        
        [DataMember]
        public Amount Amount { get; set; }

        /// <summary>
        /// Status: Estado del Extracto
        /// </summary>        
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// UserId: Usuario
        /// </summary>        
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// LocalAmount
        /// </summary>        
        [DataMember]
        public Amount LocalAmount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>        
        [DataMember]
        public ExchangeRate ExchangeRate { get; set; }
    }
}
