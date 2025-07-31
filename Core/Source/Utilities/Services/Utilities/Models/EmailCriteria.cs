using Sistran.Core.Application.UtilitiesServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    [DataContract]
    public class EmailCriteria : BaseEmailCriteria
    {

        /// <summary>
        /// lista de direcciones "destinatarios" al cual sera enviado el correo
        /// </summary>
        [DataMember]
        public List<string> Addressed { get; set; }

        /// <summary>
        /// lista de archivos "ruta fisica del archivo"
        /// </summary>
        [DataMember]
        public List<string> Files { get; set; }
    }
}
