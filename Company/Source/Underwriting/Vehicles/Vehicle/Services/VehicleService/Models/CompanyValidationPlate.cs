
using Sistran.Core.Application.ModelServices.Models.Param;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Models
{
    public class CompanyValidationPlate : ErrorServiceModel
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
    }
}
