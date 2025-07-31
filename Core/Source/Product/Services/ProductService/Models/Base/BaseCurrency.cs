namespace Sistran.Core.Application.ProductServices.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    public class BaseCurrency : Extension
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
    }
}
