using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    [DataContract]
    public class MassiveLoadVehicleReception
    {
        /// <summary>
        ///  Obtiene o establece el identificador del cargue.
        /// </summary>
        [DataMember]
        public int? MassiveLoadId { get; set; }
        
        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad MassiveLoadVehiclePolicyId.
        /// </summary>
        [DataMember]
        public int? MassiveLoadVehiclePolicyId { get; set; }
       
        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad MassiveLoadTypeCode.
        /// </summary>
        [DataMember]
        public int? MassiveLoadTypeCode { get; set; }
        
        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad LicensePlate.
        /// </summary>
        [DataMember]
        public string LicensePlate { get; set; }

        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string DescriptionType { get; set; }

        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad DescriptionMake.
        /// </summary>
        [DataMember]
        public string DescriptionMake { get; set; }
        
        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad ErrorId.
        /// </summary>
        [DataMember]
        public string ErrorId { get; set; }
    }
}
