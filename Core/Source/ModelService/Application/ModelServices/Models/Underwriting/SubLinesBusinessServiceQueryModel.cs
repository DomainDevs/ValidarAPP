namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase pública deductibleUnit
    /// </summary>
    [DataContract]
    public class SubLinesBusinessServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el modelo LineBusiness
        /// </summary>
        [DataMember]
        public List<SubLineBusinessServiceQueryModel> SubLineBusinessServiceModel { get; set; }
    }
}
