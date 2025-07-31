using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    /// <summary>
    /// Informacion Laboral de la Persona
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.V1.Models.Person" />
    [DataContract]
    public class CompanyLabourPerson : BaseLabourPerson
    {
        /// <summary>
        /// Gets or sets the occupation.
        /// </summary>
        /// <value>
        /// The occupation.
        /// </value>
        [DataMember]
        public CompanyOccupation Occupation { get; set; }

        /// <summary>
        /// Gets or sets the other occupation.
        /// </summary>
        /// <value>
        /// The other occupation.
        /// </value>
        [DataMember]
        public CompanyOccupation OtherOccupation { get; set; }

        /// <summary>
        /// Gets or sets the income level.
        /// </summary>
        /// <value>
        /// The income level.
        /// </value>
        [DataMember]
        public CompanyIncomeLevel IncomeLevel { get; set; }

        /// <summary>
        /// Gets or sets the company phone.
        /// </summary>
        /// <value>
        /// The company phone.
        /// </value>
        [DataMember]
        public CompanyPhone CompanyPhone { get; set; }

        /// <summary>
        /// Gets or sets the speciality.
        /// </summary>
        /// <value>
        /// The speciality.
        /// </value>
        [DataMember]
        public CompanySpeciality Speciality { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<CompanyPersonInterestGroup> PersonInterestGroup { get; set; }

        /// <summary>
        /// HouseType
        /// </summary>
        [DataMember]
        public CompanyHouseType HouseType { get; set; }

        /// <summary>
        /// EducationLevel
        /// </summary>
        [DataMember]
        public CompanyEducativeLevel EducativeLevel { get; set; }

        /// <summary>
        /// SocialLayer
        /// </summary>
        [DataMember]
        public CompanySocialLayer SocialLayer { get; set; }

        /// <summary>
        /// SocialLayer
        /// </summary>
        [DataMember]
        public CompanyPersonType PersonType { get; set; }
    }
}
