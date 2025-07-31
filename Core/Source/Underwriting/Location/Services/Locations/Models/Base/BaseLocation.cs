using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Locations.Models.Base
{
    [DataContract]
    public class BaseLocation : Extension
    {
        /// <summary>
        /// Obtener o setear Años de construccion
        /// </summary>
        [DataMember]
        public int ConstructionYear { get; set; }

        /// <summary>
        /// Numero de pisos
        /// </summary>
        [DataMember]
        public int FloorNumber { get; set; }

        /// <summary>
        /// Obtener o setear Tiene nomenclatura
        /// </summary>
        [DataMember]
        public bool HasNomenclature { get; set; }

        /// <summary>
        /// Obtener o setear Latitud
        /// </summary>
        [DataMember]
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Longitud
        /// </summary>
        [DataMember]
        public decimal? Longitude { get; set; }

        /// <summary>
        /// Obtener o setear Direccion
        /// </summary>
        [DataMember]
        public string FullAddress { get; set; }

        /// <summary>
        /// Obtener o setear Es declarado
        /// </summary>
        [DataMember]
        public bool IsDeclarative { get; set; }

        /// <summary>
        /// Obtener o setear PML
        /// </summary>
        [DataMember]
        public decimal? PML { get; set; }

        /// <summary>
        /// Obtener o setear Calle
        /// </summary>
        [DataMember]
        public string Square { get; set; }

        /// <summary>
        /// Obtener o Setear Edad riesgo  
        /// </summary>
        [DataMember]
        public int RiskAge { get; set; }

        /// <summary>
        /// Observaciones Reaseguros
        /// </summary>
        [DataMember]
        public string ReinsuranceObservations { get; set; }       
        

    }
}
