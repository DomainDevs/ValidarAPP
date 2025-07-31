using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class ClaimNoticeDTO
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Date 
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        [DataMember]
        public int? Number { get; set; }

        /// <summary>
        /// ObjectedNumber
        /// </summary>
        /// <param name="ObjectedNumber"></param>
        /// <returns></returns>
        [DataMember]
        public int? ObjectedNumber { get; set; }

        /// <summary>
        /// IndividualId 
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// RiskId 
        /// </summary>
        /// <param name="RiskId"></param>
        /// <returns></returns>
        [DataMember]
        public int? RiskId { get; set; }  //null

        /// <summary>
        /// PolicyId 
        /// </summary>
        /// <param name="PolicyId"></param>
        /// <returns></returns>
        [DataMember]
        public int? PolicyId { get; set; }

        /// <summary>
        /// ClaimDate 
        /// </summary>
        /// <param name="ClaimDate"></param>
        /// <returns></returns>
        [DataMember]
        public DateTime? ClaimDate { get; set; }  //null

        /// <summary>
        /// Location 
        /// </summary>
        /// <param name="Location"></param>
        /// <returns></returns>
        [DataMember]
        public string Location { get; set; }

        /// <summary>
        /// Latitude 
        /// </summary>
        [DataMember]
        public string Latitude { get; set; }

        /// <summary>
        /// Longitude 
        /// </summary>
        [DataMember]
        public string Longitude { get; set; }

        /// <summary>
        /// City 
        /// </summary>
        /// <param name="City"></param>
        /// <returns></returns>
        [DataMember]
        public CityDTO City { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// OthersAffected 
        /// </summary>
        /// <param name="OthersAffected"></param>
        /// <returns></returns>
        [DataMember]
        public string OthersAffected { get; set; }

        /// <summary>
        /// ClaimedAmount 
        /// </summary>
        /// <param name="ClaimedAmount"></param>
        /// <returns></returns>
        [DataMember]
        public decimal? ClaimedAmount { get; set; }

        /// <summary>
        /// ClaimNoticeReasonOthers
        /// </summary>
        /// <param name="ClaimNoticeReasonOthers"></param>
        /// <returns></returns>
        [DataMember]
        public string ClaimNoticeReasonOthers { get; set; }

        /// <summary>
        /// EndorsementId
        /// </summary>
        /// <param name="EndorsementId"></param>
        /// <returns></returns>
        [DataMember]
        public int? EndorsementId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [DataMember]
        public int? UserId { get; set; }

        /// <summary>
        /// Lista  de Documentos
        /// </summary>        
        /// <returns></returns>
        [DataMember]
        List<DocumentationDTO> Documents { get; set; }
    }
}
