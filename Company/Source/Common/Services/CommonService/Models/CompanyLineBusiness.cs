using Sistran.Core.Application.CommonService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.CommonServices.Models
{
    /// <summary>
    /// Ramo tecnico
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Base.BaseLineBusiness" />
    [DataContract]
    public class CompanyLineBusiness : BaseLineBusiness
    {
        /// <summary>
        /// Lista para los Objetos de seguro del ramo tecnico
        /// </summary>
        [DataMember]
        public List<int> ListInsurectObjects { get; set; }

    }
}
