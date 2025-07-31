using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Comisión
    /// </summary>
    [DataContract]
    public class Commission : BaseCommission
    {
        /// <summary>
        /// Sub Ramo Técnico
        /// </summary>
        [DataMember]
        public SubLineBusiness SubLineBusiness { get; set; }


    }
}