using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Unidad de Deducible
    /// </summary>
    [DataContract]
    public class BaseDeductibleUnit : Extension
    {
        /// <summary>
        /// Obtiene o establece el Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }


        /// <summary>
        /// Atributo para la propiedad HasSubject
        /// </summary> 
        [DataMember]
        public bool HasSubject { get; set; }
    }
}
