using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Textos
    /// </summary>
    [DataContract]
    public class BaseText:Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Titulo
        /// </summary>
        [DataMember]
        public string TextTitle { get; set; }

        /// <summary>
        /// Cuerpo
        /// </summary>
        [DataMember]
        public string TextBody { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [DataMember]
        public string Observations { get; set; }
    }
}
