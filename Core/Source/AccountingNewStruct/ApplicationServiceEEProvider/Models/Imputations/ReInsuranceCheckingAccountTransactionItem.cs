//Sistran
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// ReInsuranceCheckingAccountTransactionItem:  Transaccion de Cuenta Corriente Reaseguros
    /// </summary>
    [DataContract]
    public class ReInsuranceCheckingAccountTransactionItem : CheckingAccountTransaction
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Holder:  Compania Reaseguradora
        /// </summary>        
        [DataMember]
        public Company Holder
        {
            get;
            set;
        }

        /// <summary>
        /// Broker:  
        /// </summary>        
        [DataMember]
        public Company Broker
        {
            get;
            set;
        }

        /// <summary>
        /// Prefix:  Ramo, Subramo: Se llenara con datos de ramo, subramo tecnico 
        /// </summary>        
        [DataMember]
        public Prefix Prefix { get; set; }

        /// <summary>
        /// Year:  Anio que Aplica la Transacción
        /// </summary>        
        [DataMember]
        public int Year { get; set; }

        /// <summary>
        /// Month: Mes que Aplica la Transacción
        /// </summary>        
        [DataMember]
        public int Month { get; set; }

        /// <summary>
        /// IsFacultative: es facultativo el contrato
        /// </summary>        
        [DataMember]
        public bool IsFacultative { get; set; }

        /// <summary>
        /// ContractTypeId: Tipo de contrato
        /// </summary>        
        [DataMember]
        public int ContractTypeId { get; set; }

        /// <summary>
        /// ContractNumber: Número de contrato
        /// </summary>        
        [DataMember]
        public string ContractNumber { get; set; }

        /// <summary>
        /// SlipNumber: Número de slip
        /// </summary>        
        [DataMember]
        public string SlipNumber { get; set; }

        /// <summary>
        /// Section: Tramo del contrato
        /// </summary>        
        [DataMember]
        public string Section { get; set; }

        /// <summary>
        /// Region: 
        /// </summary>        
        [DataMember]
        public string Region { get; set; }

        /// <summary>
        /// Period: Periodo del ejercicio
        /// </summary>        
        [DataMember]
        public int Period { get; set; }

        /// <summary>
        /// EndorsementId: Número de endoso de la póliza
        /// </summary>        
        [DataMember]
        public int EndorsementId { get; set; }

        /// <summary>
        /// TransactionNumber: Número de items
        /// </summary>        
        [DataMember]
        public int TransactionNumber { get; set; }

        /// <summary>
        /// ReInsuranceCheckingAccountTransactionChild: Relacion Cuentas Corrientes ReAseguros
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public List<ReInsuranceCheckingAccountTransactionItem> ReInsuranceCheckingAccountTransactionChild
        {
            get;
            set;
        }

        /// <summary>
        /// ReinsuranceCheckingAccountItems: Relación Cuentas Corrientes de reaseguros
        /// </summary>
        [DataMember]
        public List<ReinsuranceCheckingAccountItem> ReinsurancesCheckingAccountItems
        {
            get;
            set;
        }

        /// <summary>
        /// IsAutomatic:  Si se genera de manera automatica el registro de cuenta corriente
        /// </summary>        
        [DataMember]
        public bool IsAutomatic { get; set; }

       

    }
}
