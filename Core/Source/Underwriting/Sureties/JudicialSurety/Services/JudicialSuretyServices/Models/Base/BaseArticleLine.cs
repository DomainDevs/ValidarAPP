

using System.Runtime.Serialization;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.Models.Base
{
    [DataContract]
    public class BaseArticleLine
    {
        [DataMember]
        public int ArticleLineCd { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
    }
}
