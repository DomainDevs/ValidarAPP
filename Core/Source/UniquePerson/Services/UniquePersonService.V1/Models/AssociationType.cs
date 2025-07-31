using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Tipo Asociacion
    /// </summary>
    [DataContract]
    public class AssociationType : BaseGeneric
    {

        /// <summary>
        /// Tipo de asociación
        /// </summary>
        [DataMember]
        public string NitAssociationType { get; set; }
    }
}