using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseSupplier : Extension
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int Id { get; set; }


        /// <summary>
        /// Name 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string Name { get; set; }
    }
}
