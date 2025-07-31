using Sistran.Company.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{

    [DataContract]
    public class CompanyPolicyControl
    {
        /// <summary>
        /// Identificador Temporario
        /// </summary>        
        [DataMember]
        public int TempId { get; set; }

        /// <summary>
        /// Numero de Poliza
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// Endoso id
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        /// <summary>
        /// AppSource
        /// </summary>
        [DataMember]
        public int AppSource { get; set; }

        /// <summary>
        /// Origen de la poliza 
        /// </summary>
        [DataMember]
        public PolicyOrigin PolicyOrigin { get; set; }

    }
}
