using Sistran.Core.Application.CommonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonService.Models
{
    [DataContract]
    public class SubLineBusiness : BaseSubLineBusiness
    {
        /// <summary>
        /// Ramo tecnico
        /// </summary>
        [DataMember]
        public LineBusiness LineBusiness { get; set; }
    }
}
