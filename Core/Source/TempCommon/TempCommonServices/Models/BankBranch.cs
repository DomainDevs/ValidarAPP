using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace Sistran.Core.Application.TempCommonServices.Models
{
    [DataContract]
    public class BankBranch
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
        /// IsEnabled 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public bool IsEnabled { get; set; }
    }

}
