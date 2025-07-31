#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class AccountingCompany
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