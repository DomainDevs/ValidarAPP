using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.Models
{
    [DataContract]
    public class Attorney : BaseAttorney
    {
        /// <summary>
        /// tipo de identificacion del apoderado  Numero de documento de apoderado
        /// </summary>     
        [DataMember]
        public IdentificationDocument IdentificationDocument { get; set; }
    }
}
