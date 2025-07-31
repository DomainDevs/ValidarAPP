using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;
namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyPhoneType : BaseGeneric
    {
        /// <summary>
        /// Obtiene o establece la Key para el tipo de teléfono
        /// </summary>
        [DataMember]
        public int PhoneTypeCode { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es celular
        /// </summary>
        [DataMember]
        public bool IsCellphone { get; set; }

        /// <summary>
        /// Obtiene o establece la expresión regular para validar el tipo de número
        /// </summary>
        [DataMember]
        public string RegExpression { get; set; }

        /// <summary>
        /// Obtiene o establece el mensaje de error cuando no cumpla con la expresión regular
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si tiene información asociada
        /// </summary>
        [DataMember]
        public bool IsForeing { get; set; }
    }
}
