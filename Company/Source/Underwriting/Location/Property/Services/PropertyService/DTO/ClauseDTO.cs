using Sistran.Core.Application.UnderwritingServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Location.PropertyServices.DTO
{
    /// <summary>
    /// Informaci�n de Cla�sula
    /// </summary>
    [DataContract]
	public class ClauseDTO
    {
        /// <summary>
        /// Identificador de la cla�sula
        /// </summary>
        [DataMember]
		public int Id { get; set; }

        /// <summary>
        /// Nombre de la cla�sula
        /// </summary>
        [DataMember]
		public string Name { get; set; }
        /// <summary>
        /// Titulo de la clausula
        /// </summary>
        [DataMember]
		public string Title { get; set; }
        /// <summary>
        /// Descripci�n de la cla�sula
        /// </summary>
        [DataMember]
		public string Text { get; set; }

        /// <summary>
        /// Indica si la cla�sula es obligatoria
        /// </summary>
        [DataMember]
		public bool IsMandatory { get; set; }

        /// <summary>
        /// Indica si la cla�sula ha sido seleccionada
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