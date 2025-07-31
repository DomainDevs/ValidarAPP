using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseEmailCriteria : Extension
    {
        /// <summary>
        /// Asunto del Correo 
        /// </summary>
        [DataMember]
        public string Subject { get; set; }

        /// <summary>
        /// mensaje del correo, formato HTML
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }
}
