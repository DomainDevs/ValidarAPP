using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Telefonos
    /// </summary>
    [DataContract]
    public class Phone : BasePhone
    {
        /// <summary>
        /// Tipo de tel�fono
        /// </summary>
        [DataMember]
        public PhoneType PhoneType { get; set; }
    }
}