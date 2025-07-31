using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class SupplierDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int DocumentTypeId { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public int EconomicActivityId { get; set; }
    }
}
