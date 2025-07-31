using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts
{
    /// <summary>
    ///     Tipo de Cuenta Bancaria
    /// </summary>ll
    [DataContract]
    public class BankAccountType
    {
        /// <summary>
        ///     Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Si/No Habilitado
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }
    }
}

