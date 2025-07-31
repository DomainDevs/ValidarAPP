using Sistran.Company.Application.Vehicles.VehicleApplicationService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Sistran.Company.Application.Vehicles.VehicleApplicationService.DTOs
{
    [DataContract]
    public class VehicleVersionDTO
    {
        
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int VehicleMakeServiceQueryModel { get; set; }
        [DataMember]
        public int VehicleModelServiceQueryModel { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int EngineQuantity { get; set; }
        [DataMember]
        public int HorsePower { get; set; }
        [DataMember]
        public int Weight { get; set; }

        [DataMember]
        public int TonsQuantity { get; set; }
        [DataMember]
        public int PassengerQuantity { get; set; }
        [DataMember]
        public int? VehicleFuelServiceQueryModel { get; set; }
        [DataMember]
        public int VehicleBodyServiceQueryModel { get; set; }
        [DataMember]
        public int VehicleTypeServiceQueryModel { get; set; }
        [DataMember]
        public int? VehicleTransmissionTypeServiceQueryModel { get; set; }
        [DataMember]
        public int MaxSpeedQuantity { get; set; }
        [DataMember]
        public int DoorQuantity { get; set; }

        [DataMember]
        public decimal Price { get; set; }
        [DataMember]       
        public bool IsImported { get; set; }
        [DataMember]
        public bool LastModel { get; set; }
        [DataMember]
        public int? Currency { get; set; }

        [DataMember]
        public StatusTypeService StatusTypeService { get; set; }


        /// <summary>
        /// Obtiene o establece la descripcion de la marca
        /// </summary>   
        [DataMember]
        public string DescriptionMake { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion del modelo
        /// </summary> 
        [DataMember]
        public string DescriptionModel { get; set; }

        /// <summary>
        /// Define si tiene poliza electronica 
        /// </summary>
        [DataMember]
        public bool IsElectronicPolicy { get; set; }
    }
}
