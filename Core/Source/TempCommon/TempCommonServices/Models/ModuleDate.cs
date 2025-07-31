using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.TempCommonServices.Models
{

    [DataContract]
    public class ModuleDate
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
        /// CeilingYyyy 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int CeilingYyyy { get; set; }

        /// <summary>
        /// CeilingMm 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int CeilingMm { get; set; }

    
        /// <summary>
        /// LastClosingYyyy 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int LastClosingYyyy { get; set; }

        /// <summary>
        /// LastClosingMm 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int LastClosingMm { get; set; }

        /// <summary>
        /// OfflineCeilingYyyy 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int OfflineCeilingYyyy { get; set; }

        /// <summary>
        /// OfflineCeilingMm 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int OfflineCeilingMm { get; set; }
    }
}
