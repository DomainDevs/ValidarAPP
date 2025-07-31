using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.PrintingServices.Models
{
    [DataContract]
    public class CompanyPrintingLog
    {
        [DataMember]
        public int Id;
        [DataMember]
        public int PrintingId;
        [DataMember]
        public string UrlFile;
        [DataMember]
        public int StatusCd;
    }
}
