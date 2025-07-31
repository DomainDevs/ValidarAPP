using System.Runtime.Serialization;


namespace Sistran.Core.Application.UniqueUserServices.Models
{
    [DataContract]
    public class ProfileAccesPermissions
    {
        /// <summary>
        /// key
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Id  
        /// </summary>
        [DataMember]
        public int AccessPermissionsId { get; set; }
        /// <summary>
        /// Id  Perfil
        /// </summary>
        [DataMember]
        public int ProfileId { get; set; }
    }
}
