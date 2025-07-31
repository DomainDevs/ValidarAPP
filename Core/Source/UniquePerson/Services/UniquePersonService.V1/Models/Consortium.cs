using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
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
        ///// <summary>
        ///// Obtiene O seta Compañia
        ///// </summary>
        //[DataMember]
        //public Company Company { get; set; }
        ///// <summary>
        ///// Setea o Obtiene Una Persona
        ///// </summary>
        //[DataMember]
        //public Person Person { get; set; }
    }
}
