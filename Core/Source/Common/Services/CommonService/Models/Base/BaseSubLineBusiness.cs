using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseSubLineBusiness : Extension
    {
        /// <summary>
        /// Identificador 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Sub ramo tecnico
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Sub ramo tecnico descripción corta
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Id Ramo Tecnico
        /// </summary>
        [DataMember]
        public int LineBusinessId { get; set; }

        /// <summary>
        /// Estado del modulo(modificado o eliminado)
        /// </summary>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// Ramo técnico descripción corta
        /// </summary>
        [DataMember]
        public string LineBusinessDescription { get; set; }
    }
}
