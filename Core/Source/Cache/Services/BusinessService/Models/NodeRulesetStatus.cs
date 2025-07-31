using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Cache.CacheBusinessService.Models
{
    [DataContract]
    public class NodeRulesetStatus 
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }


        /// <summary>
        /// Node
        /// </summary>
        [DataMember]
        public string Node { get; set; }

        /// <summary>
        /// Guid
        /// </summary>
        [DataMember]
        public string Guid { get; set; }

        /// <summary>
        /// Datetime
        /// </summary>
        [DataMember]
        public DateTime CreationDate { get; set; }

		/// <summary>
		/// Datetime
		/// </summary>
		[DataMember]
		public DateTime? FinishDate { get; set; }

	}
}
