using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ReportsServices.Models
{
    [DataContract]
    public class CompanyScoreCredit
    {
        [DataMember]
        public int scoreCreditId { get; set; }

        [DataMember]
        public int individualId { get; set; }

        [DataMember]
        public int idCardTypeCd { get; set; }

        [DataMember]
        public string idCardNo { get; set; }

        [DataMember]
        public string score { get; set; }

        [DataMember]
        public int responseCode { get; set; }

        [DataMember]
        public string response { get; set; }

        [DataMember]
        public DateTime dateRequest { get; set; }

        [DataMember]
        public bool isDefaultValue { get; set; }

        [DataMember]
        public int userId { get; set; }

        [DataMember]
        public string a1 { get; set; }

        [DataMember]
        public string a2 { get; set; }

        [DataMember]
        public string a3 { get; set; }

        [DataMember]   
        public string a4 { get; set; }

        [DataMember]
        public string a5 { get; set; }

        [DataMember]
        public string a6 { get; set; }

        [DataMember]
        public string a7 { get; set; }

        [DataMember]
        public string a8 { get; set; }

        [DataMember]
        public string a9 { get; set; }

        [DataMember]
        public string a10 { get; set; }

        [DataMember]
        public string a11 { get; set; }

        [DataMember]
        public string a12 { get; set; }

        [DataMember]
        public string a13 { get; set; }        

        [DataMember]
        public string a14 { get; set; }

        [DataMember]
        public string a15 { get; set; }

        [DataMember]
        public string a16 { get; set; }

        [DataMember]
        public string a17 { get; set; }

        [DataMember]
        public string a18 { get; set; }

        [DataMember]
        public string a19 { get; set; }

        [DataMember]
        public string a20 { get; set; }

        [DataMember]
        public string a21 { get; set; }

        [DataMember]
        public string a22 { get; set; }

        [DataMember]
        public string a23 { get; set; }

        [DataMember]
        public string a24 { get; set; }

        [DataMember]
        public string a25 { get; set; }

        [DataMember]
        public string request { get; set; }

        [DataMember]
        public string UserName { get; set; }
    }
}
