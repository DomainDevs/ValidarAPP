namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    /// <summary>
    /// BaseParamConditionTextLevel.
    /// </summary>
    [DataContract]
    public class BaseConditionTextLevel:Extension
    {
        /// <summary>
        /// Obtiene o establece el id de la cobertura
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el id de la cobertura
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
