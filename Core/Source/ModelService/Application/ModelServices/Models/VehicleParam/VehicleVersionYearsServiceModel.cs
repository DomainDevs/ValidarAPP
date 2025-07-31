namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public class VehicleVersionYearsServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece los los valores de los vehiculos por año
        /// </summary>
        [DataMember]
        public List<VehicleVersionYearServiceModel> VehicleVersionYearServiceModels { get; set; }
    }
}
