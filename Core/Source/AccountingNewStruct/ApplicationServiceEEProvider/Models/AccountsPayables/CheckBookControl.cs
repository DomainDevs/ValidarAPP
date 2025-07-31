using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

//SISTRAN
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables
{
    /// <summary>
    /// ActionType: 
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class CheckBookControl
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// AccountBank: Cuenta Bancaria de la Compañia
        /// </summary>        
        [DataMember]
        public BankAccountCompany AccountBank { get; set; }

        /// <summary>
        /// Description: Descripción de la Acción
        /// </summary>        
        [DataMember]
        public bool IsAutomatic { get; set; }

        /// <summary>
        /// CheckFrom: Numero de Cheque inicial
        /// </summary>        
        [DataMember]
        public int CheckFrom { get; set; }

        /// <summary>
        /// CheckTo: Numero de Cheque final
        /// </summary>        
        [DataMember]
        public int CheckTo { get; set; }

        /// <summary>
        /// LastCheck: Ultimo Cheque usado
        /// </summary>        
        [DataMember]
        public int LastCheck { get; set; }

        /// <summary>
        /// LastCheck: Ultimo Cheque usado
        /// </summary>        
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// DisabledDate: Fecha para desactivar
        /// </summary>        
        [DataMember]
        public DateTime? DisabledDate  { get; set; }
    }
}
