//Sistran

using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// BrokersCheckingAccountTransactionItem:  Transaccion de Cuenta Corriente Agentes
    /// </summary>
    [DataContract]
    public class BrokersCheckingAccountTransactionItem : CheckingAccountTransaction
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
        public new Agent Holder { get; set; }

        /// <summary>
        /// Holder:  Agente
        /// </summary>        
        [DataMember]
        public new List<Agency> Agencies{ get; set; }
        

        /// <summary>
        /// BrokersCheckingAccountTransactionChild: Relacion Cuentas Corrientes de Agentes
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public List<BrokersCheckingAccountTransactionItem> BrokersCheckingAccountTransactionChild
        {
            get;
            set;
        }

        /// <summary>
        /// BrokersCheckingAccountItems: Relación Cuentas Corrientes de Agentes
        /// </summary>
        [DataMember]
        public List<Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations.BrokerCheckingAccountItem> BrokersCheckingAccountItems
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
        public Policy Policy { get; set; }

        /// <summary>
        /// DeductCommission : Total Comision Descontada, campo para transaccion automatica
        /// </summary>        
        [DataMember]
        public Amount DeductCommission { get; set; }

        /// <summary>
        /// Prefix : Prefix, Linea de Negocio
        /// </summary>        
        [DataMember]
        public LineBusiness Prefix { get; set; }


        /// <summary>
        /// SubPrefix : SubPrefix, SubLinea de Negocio
        /// </summary>        
        [DataMember]
        public SubLineBusiness SubPrefix { get; set; }

        /// <summary>
        /// IsPayed: Esta pagado
        /// </summary>        
        [DataMember]
        public bool IsPayed { get; set; }

  





    }
}
