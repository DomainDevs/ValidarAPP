using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class ThirdPartyVehicleDTO
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Plate 
        /// </summary>
        [DataMember]
        public string Plate { get; set; }

        /// <summary>
        /// VehicleMake 
        /// </summary>
        //VALIDAR
        [DataMember]
        public string VehicleMake { get; set; }

        /// <summary>
        /// VehicleModel
        /// </summary>
        [DataMember]
        public string VehicleModel { get; set; }

        /// <summary>
        /// VehicleYear
        /// </summary>
        [DataMember]
        public int VehicleYear { get; set; }

        /// <summary>
        /// VehicleColor
        /// </summary>
        [DataMember]
        public int VehicleColorId { get; set; }

        /// <summary>
        /// VehicleColor
        /// </summary>
        [DataMember]
        public string VehicleColorDescription { get; set; }

        /// <summary>
        /// ChasisNumber
        /// </summary>
        [DataMember]
        public string ChasisNumber { get; set; }

        /// <summary>
        /// EngineNumber
        /// </summary>
        [DataMember]
        public string EngineNumber { get; set; }

        /// <summary>
        /// VinCode
        /// </summary>
        [DataMember]
        public string VinCode { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
