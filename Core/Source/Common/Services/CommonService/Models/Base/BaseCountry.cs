using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseCountry : Extension
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
