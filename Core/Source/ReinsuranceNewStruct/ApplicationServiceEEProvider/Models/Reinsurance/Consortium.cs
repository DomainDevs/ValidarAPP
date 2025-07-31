using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class Consortium: Individual
    {
        /// <summary>
        /// Obtione o setea el asegurado
        /// </summary>
        [DataMember]
        public int InsuredCode { get; set; }

        /// <summary>
        /// Obtiene o sera el Id del Consorcio
        /// </summary>
        [DataMember]
        public int ConsortiumId { get; set; }
        /// <summary>
        /// Obtiene o Setea Si es Principal
        /// </summary>
        [DataMember]
        public bool Ismain { get; set; }
        /// <summary>
        /// Obtione o Seta porcentaje de participación
        /// </summary>
        [DataMember]
        public decimal ParticipationRate { get; set; }
        /// <summary>
        /// Obtiene o seta Fecha o Inicio 
        /// </summary>
        [DataMember]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Obtione o seta Si el Consorcio esta habilitado ó Inahibilatado
        /// </summary>
        public bool Enabled { get; set; }
    }
}
