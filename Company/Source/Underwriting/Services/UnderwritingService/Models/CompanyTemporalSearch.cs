using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{

    [DataContract]
    public class CompanyTemporalSearch
    {
        /// <summary>
        /// Identificador Temporario
        /// </summary>        
        [DataMember]
        public string NumberTemporary { get; set; }

        /// <summary>
        /// Numero de Poliza
        /// </summary>
        [DataMember]
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Ramo comercial
        /// </summary>
        [DataMember]
        public string PrefixCommercial { get; set; }

        /// <summary>
        /// Asegurado
        /// </summary>
        [DataMember]
        public string Insured { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public string Branch { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [DataMember]
        public string User { get; set; }

        /// <summary>
        /// fecha Consulta
        /// </summary>
        [DataMember]
        public DateTime ConsultationDate { get; set; }

        /// <summary>
        /// Dias
        /// </summary>
        [DataMember]
        public int Days { get; set; }

        /// <summary>
        /// Intermediario principal
        /// </summary>
        [DataMember]
        public string AgentPrincipal { get; set; }

        /// <summary>
        /// Tipo de Transaccion
        /// </summary>
        [DataMember]
        public string TypeTransaction { get; set; }
    }
}
