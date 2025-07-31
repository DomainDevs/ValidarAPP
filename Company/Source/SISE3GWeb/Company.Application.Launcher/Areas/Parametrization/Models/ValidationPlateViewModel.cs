using Sistran.Core.Application.ModelServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    /// <summary>
    /// Modelo para validacion de vehiculos
    /// </summary>
    public class ValidationPlateViewModel
    {
        [DataMember]
        public int Id{ get; set; }
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
        public string  Chassis { get; set; }
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

        public StatusTypeService Status { get; set; }
    }
}