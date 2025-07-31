using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Compañia
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.Models.Individual" />
    [DataContract]
    public class Company : Individual
    {
        /// <summary>
        /// País de origen
        /// </summary>
        [DataMember]
        public Country CountryOrigin { get; set; }



        /// <summary>
        /// Tipo de compañia
        /// </summary>
        [DataMember]
        public CompanyType CompanyType { get; set; }

        /// <summary>
        /// Lista de direcciones
        /// </summary>
        [DataMember]
        public List<Address> Addresses { get; set; }

        /// <summary>
        /// Lista de teléfonos
        /// </summary>
        [DataMember]
        public List<Phone> Phones { get; set; }

        /// <summary>
        /// Lista de emails
        /// </summary>
        [DataMember]
        public List<Email> Emails { get; set; }

        /// <summary>
        /// Representante legal
        /// </summary>
        [DataMember]
        public LegalRepresentative LegalRepresentative { get; set; }

        /// <summary>
        /// Lista de accionistas
        /// </summary>
        [DataMember]
        public List<Partner> Partners { get; set; }

        /// <summary>
        /// Cheque a Nombre de
        /// </summary>
        /// <value>
        /// Nombre Corto para el asegurado o agente que va a cobrar el cheque
        /// </value>
        [DataMember]
        public string CheckPayable { get; set; }

        /// <summary>
        /// Cuenta Metodo de Pago
        /// </summary>
        [DataMember]
        public List<PaymentMethodAccount> PaymentMethodAccount { get; set; }

        /// <summary>
        /// Direcciones de Notificación
        /// </summary>
        [DataMember]
        public List<CompanyName> CompanyNames { get; set; }

        [DataMember]
        public CompanyExtended CompanyExtended { get; set; }

        /// <summary>
        /// Gets or sets the Provider
        /// </summary>        
        [DataMember]
        public Provider Provider { get; set; }

        /// <summary>
        /// Gets or sets the Provider
        /// </summary>        
        [DataMember]
        public List<IndividualTax> IndividualTax { get; set; }


        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        [DataMember]
        public List<Rol> Roles { get; set; }
    }
}
