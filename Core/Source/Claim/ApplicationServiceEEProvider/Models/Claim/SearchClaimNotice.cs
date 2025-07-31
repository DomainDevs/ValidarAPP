using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class SearchClaimNotice
    {

        [DataMember]
        public int? BranchId { get; set; }

        [DataMember]
        public int? NoticeNumber { get; set; }

        [DataMember]
        public DateTime? DateNoticeFrom { get; set; }

        [DataMember]
        public DateTime? DateNoticeTo { get; set; }

        [DataMember]
        public DateTime? DateOcurrenceFrom { get; set; }

        [DataMember]
        public DateTime? DateOcurrenceTo { get; set; }

        [DataMember]
        public int? UserId { get; set; }

        [DataMember]
        public int? IndividualId { get; set; }

        [DataMember]
        public int? HolderId { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public int RiskId { get; set; }

        [DataMember]
        public int? PrefixId { get; set; }

        [DataMember]
        public string LicensePlate { get; set; }

        [DataMember]
        public int? VehicleMakeId { get; set; }

        [DataMember]
        public int? VehicleModelId { get; set; }

        [DataMember]
        public int? VehicleVersionId { get; set; }

        [DataMember]
        public int? VehicleYear { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public int? CountryId { get; set; }

        [DataMember]
        public int? StateId { get; set; }

        [DataMember]
        public int? CityId { get; set; }

        [DataMember]
        public int? SuretyId { get; set; }

        [DataMember]
        public string CourtNumber { get; set; }

        [DataMember]
        public string BidNumber { get; set; }
    }
}
