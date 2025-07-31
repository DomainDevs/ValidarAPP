using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class InsuredDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public int DocumentTypeId { get; set; }
    }
}
