using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;
namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyAddressType : BaseGeneric
    {
        /// <summary>
        /// Obtiene o establece la Key para el tipo de dirección
        /// </summary>
        [DataMember]
        public int AddressTypeCode { get; set; }

        /// <summary>
        /// Obtiene o establece la abreviación del tipo de dirección
        /// </summary>
        [DataMember]
        public string TinyDescription { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es correo electrónico
        /// </summary>
        [DataMember]
        public bool IsElectronicMail { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si tiene información asociada
        /// </summary>
        [DataMember]
        public bool IsForeing { get; set; }
    }
}
