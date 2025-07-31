using System.Runtime.Serialization;

namespace Sistran.Core.Application.CollectiveServices.Models
{
    [DataContract]
    public class LoadType
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { set; get; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { set; get; }
    }
}