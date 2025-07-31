using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class ClaimVehicle
    {
        /// <summary>
        /// Placa
        /// </summary>
        [DataMember]
        public string LicensePlate { get; set; }

        /// <summary>
        /// Marca
        /// </summary>
        [DataMember]
        public int MakeId { get; set; }

        /// <summary>
        /// Modelo
        /// </summary>
        [DataMember]
        public int ModelId { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        [DataMember]
        public int VersionId { get; set; }

        /// <summary>
        /// Año
        /// </summary>
        [DataMember]
        public int Year { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        [DataMember]
        public int ColorId { get; set; }

        /// <summary>
        /// Nombre de Conductor
        /// </summary>
        [DataMember]
        public string DriverName { get; set; }

        /// <summary>
        /// Reclamación
        /// </summary>
        [DataMember]
        public Claim Claim { get; set; }
    }
}
