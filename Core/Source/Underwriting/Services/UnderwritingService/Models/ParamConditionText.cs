namespace Sistran.Core.Application.UnderwritingServices.Models
{
    using System.Runtime.Serialization;
    using UnderwritingServices.Models.Base;

    /// <summary>
    /// ParamConditionText.
    /// </summary>
    [DataContract]
   public  class ParamConditionText : BaseConditionText
    {
        /// <summary>
        /// Obtiene o establece id
        /// </summary>
        [DataMember]
        public BaseConditionTextLevel ConditionTextLevel { get; set; }

        /// <summary>
        /// Obtiene o establece id
        /// </summary>
        [DataMember]
        public BaseConditionTextLevelType ConditionTextLevelType { get; set; }
    }
}
