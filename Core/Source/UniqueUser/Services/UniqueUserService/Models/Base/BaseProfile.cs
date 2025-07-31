using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniqueUserServices.Models.Base
{
    [DataContract]
    public class BaseProfile : Extension
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public bool Static { get; set; }
        [DataMember]
        public bool HasAccess { get; set; }

        /// <summary>
        /// Enabled Description 
        /// </summary>
        [DataMember]
        public string EnabledDescription { get; set; }
    }
}
