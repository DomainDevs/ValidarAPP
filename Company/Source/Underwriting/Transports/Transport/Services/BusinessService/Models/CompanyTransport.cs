using System.Runtime.Serialization;
using Sistran.Core.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Core.Application.Transports.TransportBusinessService.Models;
using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.Transports.TransportBusinessService.Models.Base
{
    /// <summary>
    /// Transporte - Riesgo
    /// </summary>
    [DataContract]
    public class CompanyTransport : BaseTransport
    {
        /// <summary>
        /// Declaración estructura de transporte
        /// </summary>
        public CompanyTransport()
        {
            Risk = new CompanyRisk();
            CargoType = new CargoType();
            PackagingType = new PackagingType();
            CityFrom = new City();
            CityTo = new City();
            List<TransportType> Types = new List<TransportType>();
            ViaType = new ViaType();
            DeclarationPeriod = new DeclarationPeriod();
            AdjustPeriod = new AdjustPeriod();
        }

        /// <summary>
        /// Riesgo
        /// </summary>
        [DataMember]
        public CompanyRisk Risk { get; set; }
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
        /// <summary>
        /// Objetos del seguro
        /// </summary>
        [DataMember]
        public List<CompanyInsuredObject> InsuredObjects { get; set; }
        /// <summary>
        /// Tipo de tomador
        /// </summary>
        [DataMember]
        public CompanyHolderType HolderType { get; set; }
    }
}
