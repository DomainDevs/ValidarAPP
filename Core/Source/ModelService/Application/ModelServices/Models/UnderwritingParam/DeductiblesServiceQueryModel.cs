namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio de listado de deducibles
    /// </summary>
    [DataContract]
    public class DeductiblesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el listado de deducibles
        /// </summary>
        [DataMember]
        public List<DeductibleServiceQueryModel> DeductibleServiceQueryModels { get; set; }
    }
}
