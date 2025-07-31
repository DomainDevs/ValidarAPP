using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Sureties.SuretyServices.Models.Base;
using Sistran.Core.Application.UnderwritingServices.Models;
//using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Sureties.SuretyServices.Models
{
    [DataContract]
    public class Contract : BaseContract
    {
        /// <summary>
        /// Obtiene o setea Riesgo.
        /// </summary>
        /// <value>
        /// The risk.
        /// </value>
		[DataMember]
        public Risk Risk { get; set; }

        /// <summary>
        /// Clase de contrato
        /// </summary>
        [DataMember]
        public ContractClass Class { get; set; }

        /// <summary>
        /// Cupo operativo
        /// </summary>
        [DataMember]
        public OperatingQuota OperatingQuota { get; set; }


        /// <summary>
        /// monto
        /// </summary>
        [DataMember]
        public Amount Value { get; set; }

        /// <summary>
        /// tipo de contrato
        /// </summary>
        [DataMember]
        public ContractType ContractType { get; set; }


        /// <summary>
        /// Contratista
        /// </summary>
        [DataMember]
        public Contractor Contractor { get; set; }

        /// <summary>
        /// asegurado
        /// </summary>
        [DataMember]
        public IssuanceInsured Insured { get; set; }

        /// <summary>
        /// Contragarantias
        /// </summary>
        [DataMember]
        public List<Guarantee> Guarantees { get; set; }

        /// <summary>
        /// es facultativo
        /// </summary>
        [DataMember]
        public Boolean Isfacultative { get; set; }

        /// <summary>
        /// Objeto del contrato
        /// </summary>
        [DataMember]
        public Text ContractObject { get; set; }
    }
}
