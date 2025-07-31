using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BasePolicyType : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Tipo de poliza
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Ramo comercial
        /// </summary>
        [DataMember]
        public Prefix Prefix { get; set; }

        /// <summary>
        /// Tipo Poliza predeterminada
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }

        [DataMember]
        public bool IsFloating { get; set; }
    }
}
