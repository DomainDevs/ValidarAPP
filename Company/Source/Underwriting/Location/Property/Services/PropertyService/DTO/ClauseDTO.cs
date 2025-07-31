using Sistran.Core.Application.UnderwritingServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Location.PropertyServices.DTO
{
    /// <summary>
    /// Información de Claúsula
    /// </summary>
    [DataContract]
	public class ClauseDTO
    {
        /// <summary>
        /// Identificador de la claúsula
        /// </summary>
        [DataMember]
		public int Id { get; set; }

        /// <summary>
        /// Nombre de la claúsula
        /// </summary>
        [DataMember]
		public string Name { get; set; }
        /// <summary>
        /// Titulo de la clausula
        /// </summary>
        [DataMember]
		public string Title { get; set; }
        /// <summary>
        /// Descripción de la claúsula
        /// </summary>
        [DataMember]
		public string Text { get; set; }

        /// <summary>
        /// Indica si la claúsula es obligatoria
        /// </summary>
        [DataMember]
		public bool IsMandatory { get; set; }

        /// <summary>
        /// Indica si la claúsula ha sido seleccionada
        /// </summary>
        [DataMember]
		public bool Checked { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ConditionLevel ConditionLevel { get; set; }
        public ClauseDTO()
        {

        }
    }//end ClauseDTO


}//end namespace DTOs