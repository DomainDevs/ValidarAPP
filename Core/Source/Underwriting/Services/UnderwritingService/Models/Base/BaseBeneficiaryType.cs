using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Tipo de Beneficiario
    /// </summary>
    [DataContract]
    public class BaseBeneficiaryType : Extension
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public string TinyDescription { get; set; }
    }
}
