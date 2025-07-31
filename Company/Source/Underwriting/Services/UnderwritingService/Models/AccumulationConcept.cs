using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class AccumulationConcept
    {
        /// <summary>
        /// Id de objeto del seguro
        /// </summary>
        [DataMember]
        public int InsuredObjectId { get; set; }

        /// <summary>
        /// Id del Concepto
        /// </summary>
        [DataMember]
        public int ConceptId { get; set; }

        /// <summary>
        /// Id del entity
        /// </summary>
        [DataMember]
        public int EntityId { get; set; }
    }
}
