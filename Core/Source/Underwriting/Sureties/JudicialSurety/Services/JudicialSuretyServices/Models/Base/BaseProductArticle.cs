using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.Models.Base
{
    [DataContract]
    public class BaseProductArticle
    {
        [DataMember]
        public int ArticleId { get; set; }
        [DataMember]
        public string ArticleDescription { get; set; }
        [DataMember]
        public string ProductDescription { get; set; }
        [DataMember]
        public int ProductId { get; set; }
    }
}
