using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// BookAccount: Libro de Cuentas
    /// </summary>
    [DataContract]
    public class BookAccount
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// AccountNumber: Numero de Cuenta 
        /// </summary>        
        [DataMember]
        public string AccountNumber { get; set; }

        /// <summary>
        /// AccountName: Nombre de Cuenta
        /// </summary>        
        [DataMember]
        public string AccountName { get; set; }
    }
}
