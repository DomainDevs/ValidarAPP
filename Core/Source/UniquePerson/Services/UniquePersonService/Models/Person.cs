using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Persona
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.Models.Individual" />
    [DataContract]
    public class Person : Individual
    {
        /// <summary>
        /// Gets or sets the person code.
        /// </summary>
        /// <value>
        /// The person code.
        /// </value>
        [DataMember]
        public int PersonCode { get; set; }
        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        /// <value>
        /// The addresses.
        /// </value>
        [DataMember]
        public List<Address> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the birth place.
        /// </summary>
        /// <value>
        /// The birth place.
        /// </value>
        [DataMember]
        public string BirthPlace { get; set; }

        /// <summary>
        /// Gets or sets the emails.
        /// </summary>
        /// <value>
        /// The emails.
        /// </value>
        [DataMember]
        public List<Email> Emails { get; set; }

        /// <summary>
        /// Gets or sets the names.
        /// </summary>
        /// <value>
        /// The names.
        /// </value>
        [DataMember]
        public string Names { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        [DataMember]
        public string Gender { get; set; }
        /// <summary>
        /// Gets or sets the last name of the mother.
        /// </summary>
        /// <value>
        /// The last name of the mother.
        /// </value>
        [DataMember]
        public string MotherLastName { get; set; }
        /// <summary>
        /// Gets or sets the marital status.
        /// </summary>
        /// <value>
        /// The marital status.
        /// </value>
        [DataMember]
        public MaritalStatus MaritalStatus { get; set; }
        /// <summary>
        /// Gets or sets the phones.
        /// </summary>
        /// <value>
        /// The phones.
        /// </value>
        [DataMember]
        public List<Phone> Phones { get; set; }
        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        [DataMember]
        public int? Children { get; set; }
        /// <summary>
        /// Gets or sets the educative level.
        /// </summary>
        /// <value>
        /// The educative level.
        /// </value>
        [DataMember]
        public EducativeLevel EducativeLevel { get; set; }
        /// <summary>
        /// Gets or sets the name of the spouse.
        /// </summary>
        /// <value>
        /// The name of the spouse.
        /// </value>
        [DataMember]
        public string SpouseName { get; set; }
        /// <summary>
        /// Gets or sets the type of the house.
        /// </summary>
        /// <value>
        /// The type of the house.
        /// </value>
        [DataMember]
        public HouseType HouseType { get; set; }
        /// <summary>
        /// Gets or sets the social layer.
        /// </summary>
        /// <value>
        /// The social layer.
        /// </value>
        [DataMember]
        public SocialLayer SocialLayer { get; set; }
        /// <summary>
        /// Gets or sets the nationality.
        /// </summary>
        /// <value>
        /// The nationality.
        /// </value>
        [DataMember]
        public string Nationality { get; set; }
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        [DataMember]
        public City City { get; set; }
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        [DataMember]
        public List<Rol> Roles { get; set; }
        /// <summary>
        /// Gets or sets the payment mthod account.
        /// </summary>
        /// <value>
        /// The payment mthod account.
        /// </value>
        [DataMember]
        public List<PaymentMethodAccount> PaymentMthodAccount { get; set; }
        /// <summary>
        /// Gets or sets the labor person.
        /// </summary>
        /// <value>
        /// The labor person.
        /// </value>
        [DataMember]
        public LaborPerson LaborPerson { get; set; }

        /// <summary>
        /// Gets or sets the type of the person.
        /// </summary>
        /// <value>
        /// The type of the person.
        /// </value>
        [DataMember]
        public PersonIndividualType PersonType { get; set; }

        /// <summary>
        /// Gets or sets the Provider
        /// </summary>        
        [DataMember]
        public Provider Provider { get; set; }

        /// <summary>
        /// Gets or sets the Provider
        /// </summary>        
        [DataMember]
        public List<IndividualTaxExeption> IndividualTaxsExemption { get; set; }

        /// <summary>
        /// Tipo de persona (estudiante, empleado, pensionado)
        /// </summary>
        [DataMember]
        public int? PersonStateType { get; set; }

        /// <summary>
        /// Gets or sets DataProtection.
        /// </summary>
        [DataMember]
        public bool DataProtection { get; set; }
    }
}
