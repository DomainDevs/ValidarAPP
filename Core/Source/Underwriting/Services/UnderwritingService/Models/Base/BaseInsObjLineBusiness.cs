using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Coberturas
    /// </summary>
    [DataContract]
    public class BaseInsObjLineBusiness : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }


        /// <summary>
        /// Atributo para la propiedad LineBusinessCd
        /// </summary> 
        [DataMember]
        public int LineBusinessCd { get; set; }
    }
}
