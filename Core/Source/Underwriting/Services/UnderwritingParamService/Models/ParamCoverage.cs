// -----------------------------------------------------------------------
// <copyright file="ParamCoverage.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    /// <summary>
    /// Modelo de negocio de cobertura
    /// </summary>
    [DataContract]
    public class ParamCoverage: BaseParamCoverage
    {
       
       
        /// <summary>
        /// Obtiene o establece el ramo tecnico
        /// </summary>
        [DataMember]
        public ParamLineBusinessDesc LineBusiness { get; set; }

        /// <summary>
        /// Obtiene o establece el subramo tecnico
        /// </summary>
        [DataMember]
        public ParamSubLineBusinessDesc SubLineBusiness { get; set; }

        /// <summary>
        /// Obtiene o establece el amparo
        /// </summary>
        [DataMember]
        public ParamPeril Peril { get; set; }

        /// <summary>
        /// Obtiene o establece el objeto del seguro
        /// </summary>
        [DataMember]
        public ParamInsuredObjectDesc InsuredObjectDesc { get; set; }

        /// <summary>
        /// Obtiene o establece la relacion con la CoCoverage Principal
        /// </summary>
        [DataMember]
        public ParamCoCoverage CoCoverage { get; set; }

        /// <summary>
        /// Obtiene o establece la relacion con las CoCoverage Hijas
        /// </summary>
        [DataMember]
        public List<ParamCoCoverage> CoCoverages { get; set; }

        /// <summary>
        /// Obtiene o establece la homoloagacion de la covertura con 2G
        /// </summary>
        [DataMember]
        public ParamCoCoverage2G Homologation2G { get; set; }
    }
}
