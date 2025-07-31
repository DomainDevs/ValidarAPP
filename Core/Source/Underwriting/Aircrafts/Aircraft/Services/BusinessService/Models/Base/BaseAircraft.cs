using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Aircrafts.AircraftBusinessService.Models.Base
{
    [DataContract]
    public class BaseAircraft : Extension
    {
        /// <summary>
        /// Id de Marca
        /// </summary>
        [DataMember]
        public int MakeId { get; set; }

        /// <summary>
        /// Id de modelo
        /// </summary>
        [DataMember]
        public int ModelId { get; set; }

        /// <summary>
        /// Id de Typo de aeronave
        /// </summary>
        [DataMember]
        public int TypeId { get; set; }

        /// <summary>
        /// Id uso de aeronave
        /// </summary>
        [DataMember]
        public int UseId { get; set; }

        /// <summary>
        /// Id de matricula
        /// </summary>
        [DataMember]
        public int RegisterId { get; set; }

        /// <summary>
        /// Año de fabricación
        /// </summary>
        [DataMember]
        public int CurrentManufacturing { get; set; }
        /// <summary>
        /// Explotadores de aeronaves
        /// </summary>
        [DataMember]
        public int OperatorId { get; set; }

        /// <summary>
        /// Número de Matricula
        /// </summary>
        [DataMember]
        public string NumberRegister { get; set; }

    }
}

