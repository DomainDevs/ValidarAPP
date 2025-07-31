using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// BrokersCheckingAccountTransactionItem:  Transaccion de Cuenta Corriente Agentes
    /// </summary>
    [DataContract]
    public class BrokersCheckingAccountTransactionItemDTO : CheckingAccountTransactionDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Holder:  Agente
        /// </summary>        
        [DataMember]
        public new AgentDTO Holder { get; set; }

        /// <summary>
        /// BrokersCheckingAccountTransactionChild: Relacion Cuentas Corrientes de Agentes
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public List<BrokersCheckingAccountTransactionItemDTO> BrokersCheckingAccountTransactionChild
        {
            get;
            set;
        }

        /// <summary>
        /// BrokersCheckingAccountItems: Relación Cuentas Corrientes de Agentes
        /// </summary>
        [DataMember]
        public List<BrokerCheckingAccountItemDTO> BrokersCheckingAccountItems
        {
            get;
            set;
        }


        /// <summary>
        /// IsAutomatic:  Si se genera de manera automatica el registro de cuenta corriente
        /// </summary>        
        [DataMember]
        public bool IsAutomatic { get; set; }

        /// <summary>
        /// Policy : Poliza , campo para transaccion automatica
        /// </summary>        
        [DataMember]
        public PolicyDTO Policy { get; set; }

        /// <summary>
        /// DeductCommission : Total Comision Descontada, campo para transaccion automatica
        /// </summary>        
        [DataMember]
        public AmountDTO DeductCommission { get; set; }

        /// <summary>
        /// Prefix : Prefix, Linea de Negocio
        /// </summary>        
        [DataMember]
        public LineBusinessDTO Prefix { get; set; }


        /// <summary>
        /// SubPrefix : SubPrefix, SubLinea de Negocio
        /// </summary>        
        [DataMember]
        public SubLineBusinessDTO SubPrefix { get; set; }

        /// <summary>
        /// IsPayed: Esta pagado
        /// </summary>        
        [DataMember]
        public bool IsPayed { get; set; }

        /// <summary>
        /// BrokersCheckingAccountTransactionItems
        /// </summary>
        [DataMember]
        public List<BrokersCheckingAccountTransactionItemDTO> BrokersCheckingAccountTransactionItems { get; set; }
    }
}
