using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Salvage
{
    public class Salvage 
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime CreationDate { get; set; }

        [DataMember]
        public DateTime? AssignmentDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public decimal EstimatedSale { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string Observations { get; set; }

        [DataMember]
        public int ClaimId { get; set; }

        [DataMember]
        public int SubClaimId { get; set; }

        [DataMember]
        public int ClaimNumber { get; set; }

        [DataMember]
        public int? UnitsQuantity { get; set; }

        [DataMember]
        public decimal? TotalAmount { get; set; }


        [DataMember]
        public Prefix Prefix { get; set; }

        [DataMember]
        public Branch Branch { get; set; }

        [DataMember]
        public List<Sale> Sales { get; set; }

        [DataMember]
        public decimal RecoveryAmount { get; set; }

    }
}
