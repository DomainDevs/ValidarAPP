using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Models.Base
{
    [DataContract]
    public class BaseAccessPermissions
    {

        /// <summary>
        /// Id 
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// ModuleCode 
        /// </summary>
        [DataMember]
        public int ModuleCode { get; set; }
        /// <summary>
        /// SubmoduleCode 
        /// </summary>
        [DataMember]
        public int SubmoduleCode { get; set; }

        /// <summary>
        /// Code 
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

    }
}
