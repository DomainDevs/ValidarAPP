using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    /// <summary>
    /// Consorcios
    /// </summary>
    [DataContract]
    public class CompanyConsortium: Individual
    {
        /// <summary>
        /// Obtiene o Setea el Codigo del Asegurado
        /// </summary>
        /// <value>
        /// Codigo del Asegurado
        /// </value>
        [DataMember]
        public int InsuredCode { get; set; }

        
        /// <summary>
        /// Obtiene o Setea el Id Consorcio
        /// </summary>
        /// <value>
        /// Id Consorcio
        /// </value>
        [DataMember]
        public int ConsortiumId { get; set; }

        /// <summary>
        /// Obtiene o Setea si es principal
        /// </summary>
        /// <value>
        ///   <c>true</c> si es principal<c>false</c>.
        /// </value>
        [DataMember]
        public bool IsMain { get; set; }

        /// <summary>
        ///  Obtiene o Setea Porcentaje Participacion
        /// </summary>
        /// <value>
        /// Porcentaje Participacion
        /// </value>
        [DataMember]
        public decimal ParticipationRate { get; set; }

        /// <summary>
        /// Obtiene o Setea Fecha Inicio
        /// </summary>
        /// <value>
        /// Fecha Inicio
        /// </value>
        [DataMember]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Obtiene o Setea  <see cref="CompanyConsortium"/> si esta Habilitado
        /// </summary>
        /// <value>
        ///   <c>true</c> si esta Habilitado; deshabilitado, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Obtiene o Setea Compañia
        /// </summary>
        /// <value>
        /// Compañia
        /// </value>
        [DataMember]
        public CompanyCompany Company { get; set; }

        /// <summary>
        /// Obtiene o Setea Persona
        /// </summary>
        /// <value>
        /// Persona
        /// </value>
        [DataMember]
        public CompanyPerson Person { get; set; }
    }
}
