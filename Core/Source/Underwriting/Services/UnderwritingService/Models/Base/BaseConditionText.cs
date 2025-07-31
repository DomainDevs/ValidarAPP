namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    using System.Runtime.Serialization;
    using Sistran.Core.Application.Extensions;

    /// <summary>
    /// BaseParamConditionText.
    /// </summary>
    [DataContract]
    public class BaseConditionText : Extension
    {
        /// <summary>
        /// Obtiene o establece el id de la cobertura
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Titulo del texto
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Body.
        /// </summary>
        [DataMember]
        public string Body { get; set; }
    }
}
