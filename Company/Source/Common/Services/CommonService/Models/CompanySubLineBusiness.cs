using Sistran.Core.Application.CommonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.CommonServices.Models
{
    [DataContract]
    public class CompanySubLineBusiness : BaseSubLineBusiness
    {

        /// <summary>
        /// Ramo tecnico
        /// </summary>
        [DataMember]
        public CompanyLineBusiness LineBusiness { get; set; }
    }
}
