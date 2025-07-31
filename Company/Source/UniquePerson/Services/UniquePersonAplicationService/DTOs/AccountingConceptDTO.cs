using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class AccountingConceptDTO
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// <returns></returns>  
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// AccountingAccountId 
        /// </summary>
        /// <returns></returns> 
        [DataMember]
        public int AccountingAccountId { get; set; }

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
        [DataMember]
        public bool CoinsuranceEnable { get; set; }

        /// <summary>
        /// ReinsuranceEnable 
        /// </summary>   
        /// <returns></returns>   
        [DataMember]
        public bool ReinsuranceEnable { get; set; }

        /// <summary>
        /// InsuredEnabled 
        /// </summary>   
        /// <returns></returns> 
        [DataMember]
        public bool InsuredEnabled { get; set; }

        /// <summary>
        /// ItemEnabled 
        /// </summary>   
        /// <returns></returns>   
        [DataMember]
        public bool ItemEnabled { get; set; }
    }
}
