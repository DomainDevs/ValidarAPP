using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Transports.TransportBusinessService.Models
{
    /// <summary>
    /// Transporte - Riesgo
    /// </summary>
    [DataContract]
    public class Transport : BaseTransport
    {
        public Transport()
        {
            Risk = new Risk();
            CargoType = new CargoType();
            PackagingType = new PackagingType();
            CityFrom = new City();
            CityTo = new City();
            List<TransportType> Types = new List<TransportType>();
            ViaType = new ViaType();
            DeclarationPeriod = new DeclarationPeriod();
            AdjustPeriod = new AdjustPeriod();
        }

        [DataMember]
        public Risk Risk { get; set; }
        /// <summary>
        /// Tipo cargamento o mercancia 
        /// </summary>
        [DataMember]
        public CargoType CargoType { get; set; }
        /// <summary>
        /// Tipo de paquete o empaque
        /// </summary>
        [DataMember]
        public PackagingType PackagingType { get; set; }
        /// <summary>
        /// Desde
        /// </summary>
        [DataMember]
        public City CityFrom { get; set; }
        /// <summary>
        /// Hasta
        /// </summary>
        [DataMember]
        public City CityTo { get; set; }
        /// <summary>
        /// Medio de Transporte
        /// </summary>
        [DataMember]
        public List<TransportType> Types { get; set; }
        /// <summary>
        /// Via 
        /// </summary>
        [DataMember]
        public ViaType ViaType { get; set; }
        /// <summary>
        /// Periodo de Declaración
        /// </summary>
        [DataMember]
        public DeclarationPeriod DeclarationPeriod { get; set; }
        /// <summary>
        /// Periodo de Ajuste 
        /// </summary>
        [DataMember]
        public AdjustPeriod AdjustPeriod { get; set; }
    }
}
