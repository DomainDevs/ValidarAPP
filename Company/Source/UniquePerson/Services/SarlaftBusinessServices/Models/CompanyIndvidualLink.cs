using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{
    [DataContract]
    public class CompanyIndvidualLink
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public CompanyLinkType LinkType { get; set; }
        [DataMember]
        public CompanyRelationShip RelationshipSarlaft { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int SarlaftId { get; set; }
    }
}
