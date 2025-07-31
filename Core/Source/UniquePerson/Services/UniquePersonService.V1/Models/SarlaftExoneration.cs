using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    [DataContract]
    public class SarlaftExoneration : BaseSarlaftExoneration
    {
        /// <summary>
        /// Tipo exoneracion
        /// </summary>
        [DataMember]
        public ExonerationType ExonerationType { get; set; }

        public SarlaftExoneration() { }
    }
}
