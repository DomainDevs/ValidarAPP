using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseAccountingConcept
    {

        /// <summary>
        /// Id 
        /// </summary>
        /// <returns></returns>  
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        /// <returns></returns>      
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// AgentEnabled 
        /// </summary>   
        /// <returns></returns>       
        [DataMember]
        public bool AgentEnabled { get; set; }

        /// <summary>
        /// CoinsuranceEnable 
        /// </summary>   
        /// <returns></returns>       
        public bool CoinsuranceEnable { get; set; }

        /// <summary>
        /// ReinsuranceEnable 
        /// </summary>   
        /// <returns></returns>       
        public bool ReinsuranceEnable { get; set; }

        /// <summary>
        /// InsuredEnabled 
        /// </summary>   
        /// <returns></returns>       
        public bool InsuredEnabled { get; set; }

        /// <summary>
        /// ItemEnabled 
        /// </summary>   
        /// <returns></returns>       
        public bool ItemEnabled { get; set; }

    }
}
