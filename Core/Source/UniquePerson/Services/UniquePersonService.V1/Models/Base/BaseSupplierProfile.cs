using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseSupplierProfile 
    {

        /// <summary>
        /// Id 
        /// </summary>
        /// <returns></returns>      
        public int Id { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        /// <returns></returns>      
        public string Description { get; set; }

        /// <summary>
        /// IsEnabled 
        /// </summary>
        /// <param name="Enabled"></param>
        /// <returns></returns>       
        public bool IsEnabled { get; set; }


    }
}
