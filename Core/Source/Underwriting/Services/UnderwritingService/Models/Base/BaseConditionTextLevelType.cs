
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// BaseParamConditionTextLevel.
    /// </summary>
    [DataContract]
    public class BaseConditionTextLevelType: Extension
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
