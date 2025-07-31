using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class Cause
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Requiere Denuncia Policial
        /// </summary>
        [DataMember]
        public bool PoliceComplaintRequired { get; set; }

        /// <summary>
        /// Requiere Información del Conductor
        /// </summary>
        [DataMember]
        public bool DriverInformationRequired { get; set; }

        /// <summary>
        /// Requiere Fecha de Inspección
        /// </summary>
        [DataMember]
        public bool InspectionDateRequired { get; set; }

        /// <summary>
        /// Ramo Comercial
        /// </summary>
        [DataMember]
        public Prefix Prefix { get; set; }
    }
}
