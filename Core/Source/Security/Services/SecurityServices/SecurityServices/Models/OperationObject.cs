using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.SecurityServices.Models
{
    /// <summary>
    /// Objetos de los Accesos
    /// </summary>
    [DataContract]
    public class OperationObject : Extension
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Url 
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        [DataMember]
        public string Route { get; set; }

        /// <summary>
        /// Enable 
        /// </summary>
        /// <param name="Enable"></param>
        /// <returns></returns>
        [DataMember]
        public bool Enable { get; set; }

        /// <summary>
        /// Visible 
        /// </summary>
        /// <param name="Visible"></param>
        /// <returns></returns>
        [DataMember]
        public bool Visible { get; set; }
    }
}
