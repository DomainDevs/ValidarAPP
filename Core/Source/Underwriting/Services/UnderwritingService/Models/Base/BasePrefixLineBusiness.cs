using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Ramo tecnico ramo comercial
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BasePrefixLineBusiness:Extension
    {
        [DataMember]
        public int PrefixCode { get; set; }
        [DataMember]
        public int LineBusinessCode { get; set; }
        [DataMember]
        public string Status { get; set; }
    }
}
