using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Informacion Laboral de la Persona
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.Models.Person" />
    [DataContract]
    public class LaborPerson : BaseLaborPerson
    {
        /// <summary>
        /// Gets or sets the occupation.
        /// </summary>
        /// <value>
        /// The occupation.
        /// </value>
        [DataMember]
        public Occupation Occupation { get; set; }

        /// <summary>
        /// Gets or sets the other occupation.
        /// </summary>
        /// <value>
        /// The other occupation.
        /// </value>
        [DataMember]
        public Occupation OtherOccupation { get; set; }

        /// <summary>
        /// Gets or sets the income level.
        /// </summary>
        /// <value>
        /// The income level.
        /// </value>
        [DataMember]
        public IncomeLevel IncomeLevel { get; set; }

        /// <summary>
        /// Gets or sets the company phone.
        /// </summary>
        /// <value>
        /// The company phone.
        /// </value>
        [DataMember]
        public Phone CompanyPhone { get; set; }

        /// <summary>
        /// Gets or sets the speciality.
        /// </summary>
        /// <value>
        /// The speciality.
        /// </value>
        [DataMember]
        public Speciality Speciality { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<PersonInterestGroup> PersonInterestGroup { get; set; }

    }
}
