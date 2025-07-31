using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Models
{
    [DataContract]
    public class LimitRCExecess
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Descripción { get; set; }
    }
}