namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase pública deductibleUnit
    /// </summary>
    [DataContract]
    public class LinesBusinessServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el modelo LineBusiness
        /// </summary>
        [DataMember]
        public List<LineBusinessServiceQueryModel> LineBusinessServiceModel { get; set; }
    }
}
