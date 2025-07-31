using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseLineBusiness : Extension
    {
        /// <summary>
        /// Identificador 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Ramo tecnico
        /// </summary>
        [DataMember]
        public string Description { get; set; }


        /// <summary>
        /// Descripcion corta
        /// </summary>
        [DataMember]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Abreviatura descripcion
        /// </summary>
        [DataMember]
        public string TyniDescription { get; set; }


        /// <summary>
        /// report line business
        /// </summary>
        [DataMember]
        public int ReportLineBusiness { get; set; }

        /// <summary>
        /// id del ramo tecnico asociado al tipo de riesgo 
        /// </summary>
        [DataMember]
        public int IdLineBusinessbyRiskType { get; set; }

        /// <summary>
        /// Estado del modulo(modificado o eliminado)
        /// </summary>
        [DataMember]
        public string Status { get; set; }
    }
}
