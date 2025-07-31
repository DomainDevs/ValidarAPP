using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ModuleDateDTO
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
