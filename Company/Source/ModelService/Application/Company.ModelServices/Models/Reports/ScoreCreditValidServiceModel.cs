using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ModelServices.Models.Reports
{
    [DataContract]
    public class ScoreCreditValidServiceModel
    {
        [DataMember]
        public int idCardTypeCd { get; set; }

        [DataMember]
        public string idCardNo { get; set; }

        [DataMember]
        public string score { get; set; }

        [DataMember]
        public DateTime dateRequest { get; set; }
    }
}
