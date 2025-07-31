using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseLegalRepresentative : Extension
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the expedition date.
        /// </summary>
        /// <value>
        /// The expedition date.
        /// </value>
        [DataMember]
        public DateTime ExpeditionDate { get; set; }

        /// <summary>
        /// Gets or sets the expedition place.
        /// </summary>
        /// <value>
        /// The expedition place.
        /// </value>
        [DataMember]
        public string ExpeditionPlace { get; set; }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        [DataMember]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the birth place.
        /// </summary>
        /// <value>
        /// The birth place.
        /// </value>
        [DataMember]
        public string BirthPlace { get; set; }

        /// <summary>
        /// Gets or sets the nationality.
        /// </summary>
        /// <value>
        /// The nationality.
        /// </value>
        [DataMember]
        public string Nationality { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        [DataMember]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
        [DataMember]
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the cell phone.
        /// </summary>
        /// <value>
        /// The cell phone.
        /// </value>
        [DataMember]
        public string CellPhone { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Manager Name.
        /// </summary>
        /// <value>
        /// The  Manager Name.
        /// </value>
        [DataMember]
        public string ManagerName { get; set; }

        /// <summary>
        /// Gets or sets the General Manager Name.
        /// </summary>
        /// <value>
        /// The General Manager Name.
        /// </value>
        [DataMember]
        public string GeneralManagerName { get; set; }

        /// <summary>
        /// Gets or sets the contact name.
        /// </summary>
        /// <value>
        /// The contact name.
        /// </value>
        [DataMember]
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the contact additional information.
        /// </summary>
        /// <value>
        /// The Contact additional information.
        /// </value>
        [DataMember]
        public string ContactAdditionalInfo { get; set; }

    }
}
