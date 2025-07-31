using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniqueUserServices.Models
{
    [DataContract]
    public class AccessProfile : Extension
    {
        /// <summary>
        /// Id 
        /// </summary>
        [DataMember]
        public int ProfileId { get; set; }

        /// <summary>
        /// Id 
        /// </summary>
        [DataMember]
        public int AccessId { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        [DataMember]
        public int DatabaseId { get; set; }

        /// <summary>
        /// Url 
        /// </summary>
        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Enable 
        /// </summary>
        [DataMember]
        public bool IsExpirationDateNull { get; set; }

        /// <summary>
        /// Visible 
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Si está asignado o no
        /// </summary>
        [DataMember]
        public bool Assigned { get; set; }

        /// <summary>
        /// Si está asignado o no
        /// </summary>
        [DataMember]
        public bool InDatabase { get; set; }

        /// <summary>
        /// Id del objeto a modificar  para opcion permisos (profileaccesspermissions)
        /// </summary>
        [DataMember]
        public int AccessObjectId { get; set; }

        /// <summary>
        /// type access
        /// </summary>
        [DataMember]
        public int AccessType { get; set; }
    }
}
