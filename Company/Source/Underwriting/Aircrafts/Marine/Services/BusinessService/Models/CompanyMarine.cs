using System.Runtime.Serialization;
using Sistran.Core.Application.Marines.MarineBusinessService.Models.Base;
using Sistran.Core.Application.Marines.MarineBusinessService.Models;
using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.Marines.MarineBusinessService.Models.Base
{
    /// <summary>
    /// Marinee - Riesgo
    /// </summary>
    [DataContract]
    public class CompanyMarine : BaseMarine
    {
        /// <summary>
        /// Declaración estructura de Marinee
        /// </summary>
        public CompanyMarine()
        {
            Risk = new CompanyRisk();
            List<MarineType> Types = new List<MarineType>();
       
        }

        /// <summary>
        /// Riesgo
        /// </summary>
        [DataMember]
        public CompanyRisk Risk { get; set; }

        /// <summary>
        /// Medio de Marinee
        /// </summary>
        [DataMember]
        public List<MarineType> Types { get; set; }

        /// <summary>
        /// Objetos del seguro
        /// </summary>
        [DataMember]
        public List<InsuredObject> InsuredObjects { get; set; }
    }
}
