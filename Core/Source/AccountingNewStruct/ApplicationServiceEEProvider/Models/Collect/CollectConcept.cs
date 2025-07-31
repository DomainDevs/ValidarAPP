using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect
{
    /// <summary>
    /// CollectConcept: Concepto de Ingreso
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class CollectConcept
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
    
        public CollectConcept()
        {
            
        }

        public CollectConcept(int id)
        {
            this.Id = id;
        }
    }
}
