using Sistran.Company.Application.Vehicles.VehicleApplicationService.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleApplicationService.DTOs
{
    public class ValidationPlateDTO
    {
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// placa
        /// </summary>
        [DataMember]
        public string Plate { get; set; }
        [DataMember]
        public string Motor { get; set; }
        /// <summary>
        /// Chasis del carro
        /// </summary>
        [DataMember]
        public string Chassis { get; set; }
        /// <summary>
        /// CODIGO FASECOLDA
        /// </summary>
        [DataMember]
        public string CodFasecolda { get; set; }
        [DataMember]
        public int CodMake { get; set; }
        [DataMember]
        public int CodModel { get; set; }
        [DataMember]
        public int CodVersion { get; set; }
        /// <summary>
        /// causa
        /// </summary>
        [DataMember]
        public int CodCause { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }

        //[DataMember]
        //public List<ValidationPlateDTO> ValidationPlateModel { get; set; }
        [DataMember]
        public StatusTypeService Status { get; set; }

    }
}
