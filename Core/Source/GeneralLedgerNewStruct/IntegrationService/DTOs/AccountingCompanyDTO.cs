using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.GeneralLedgerServices.DTOs
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class AccountingCompanyDTO
    {
        /// <summary>
        ///     Identificador único del modelo
        /// </summary>
        [DataMember]
        public int AccountingCompanyId { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     CAmpo  en  el cual  se inicia por defecto la empresa  que  se cree
        /// </summary>
        [DataMember]
        public bool Default { get; set; }
    }
}
