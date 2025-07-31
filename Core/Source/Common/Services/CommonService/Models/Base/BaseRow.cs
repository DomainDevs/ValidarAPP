using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseRow : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Número Fila
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Tiene Error?
        /// </summary>
        [DataMember]
        public bool HasError { get; set; }

        /// <summary>
        /// Descripción Error
        /// </summary>
        [DataMember]
        public string ErrorDescription { get; set; }
    }
}
