using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Clausula Base
    /// </summary>
    [DataContract]
    public class BaseClause : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Titulo
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Clausula
        /// </summary>
        [DataMember]
        public string Text { get; set; }

        /// <summary>
        /// Es Obligatoria?
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }
    }
}
