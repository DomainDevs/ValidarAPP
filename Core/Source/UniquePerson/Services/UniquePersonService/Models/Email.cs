using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Correo
    /// </summary>
    [DataContract]
    public class Email : BaseEmail
    {
        /// <summary>
        /// Tipo de email
        /// </summary>
        [DataMember]
        public EmailType EmailType { get; set; }
    }
}
