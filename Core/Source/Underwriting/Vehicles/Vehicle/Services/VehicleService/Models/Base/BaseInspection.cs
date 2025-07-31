using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.VehicleServices.Models.Base
{
    [DataContract]
    public class BaseInspection
    {

        /// <summary>
        /// Tipo
        /// </summary>
        [DataMember]
        public int InspectionType { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }


    }
}
