using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    [DataContract]
    public class Company : BaseCompany
    {
        /// <summary>
        /// role
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// EconomicActivity
        /// </summary>
        public EconomicActivity EconomicActivity { get; set; }

        /// <summary>
        /// IdentificationDocument
        /// </summary>
        public IdentificationDocument IdentificationDocument { get; set; }

        /// <summary>
        /// CompanyType
        /// </summary>
        public CompanyType CompanyType { get; set; }

        /// <summary>
        /// AssociationType
        /// </summary>
        public AssociationType AssociationType { get; set; }

        /// <summary>
        /// Insured
        /// </summary>
        public Insured Insured { get; set; }

        /// <summary>
        /// Consortiums
        /// </summary>
        public List<Consortium> Consortiums { get; set; }

        /// <summary>
        /// CheckPayable
        /// </summary>
        public string CheckPayable { get; set; }

    }
}
