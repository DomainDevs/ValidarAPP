using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class ClaimSupplierDTO
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Adjuster 
        /// </summary>
        /// <param name="Adjuster>
        /// <returns></returns>
        [DataMember]
        public int AdjusterId { get; set; }

        /// <summary>
        /// Researcher 
        /// </summary>
        /// <param name="Researcher>
        /// <returns></returns>
        [DataMember]
        public int ResearcherId { get; set; }

        /// <summary>
        /// Analizer 
        /// </summary>
        /// <param name="Analizer>
        /// <returns></returns>
        [DataMember]
        public int AnalizerId { get; set; }

        /// <summary>
        /// DateInspection  
        /// </summary>
        /// <param name="DateInspection>
        /// <returns></returns>
        [DataMember]
        public DateTime DateInspection { get; set; }

        /// <summary>
        ///  HourInspection
        /// </summary>
        /// <param name="HourInspection>
        [DataMember]
        public string HourInspection { get; set; }


        /// <summary>
        /// AffectedProperty  
        /// </summary>
        /// <param name="AffectedProperty>
        /// <returns></returns>
        [DataMember]
        public string AffectedProperty { get; set; }

        /// <summary>
        /// LossDescription  
        /// </summary>
        /// <param name="LossDescription>
        /// <returns></returns>
        [DataMember]
        public string LossDescription { get; set; }
    }
}
