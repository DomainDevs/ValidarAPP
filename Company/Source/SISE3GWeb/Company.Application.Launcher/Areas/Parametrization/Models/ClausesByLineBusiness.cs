
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class ClausesByLineBusiness
    {
        /// <summary>
        /// Id de la Clausula
        /// </summary>
        [DataMember]
        public int IdClauseByLineBusiness { get; set; }

        /// <summary>
        /// Descripcion de la clausula
        /// </summary>
        [DataMember]
        public string DescriptionClauseByLineBusiness { get; set; }

        /// <summary>
        /// Id del ramo tecnico
        /// </summary>
        [DataMember]
        public int IdLineBusinessClause { get; set; }

        /// <summary>
        /// valida si es obligatoria la clausula
        /// </summary>
        [DataMember]
        public string IsDefaultMandatory { get; set; }



        [DataMember]
        public string  IsDefaultDescription { get; set; }

        /// <summary>
        /// Variable que maneja el estado del registro de clausula
        /// Added
        /// Deleted
        /// 2 estado Eliminado
        /// </summary>
        [DataMember]
        public string Status { get; set; }

        
    }
}