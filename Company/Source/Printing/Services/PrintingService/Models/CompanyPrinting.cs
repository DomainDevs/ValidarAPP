using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.PrintingServices.Models
{
    [DataContract]
    public class CompanyPrinting
    {
        [DataMember]
        public int Id;
        [DataMember]
        public int PrintingTypeId;
        [DataMember]
        public int? KeyId;
        [DataMember]
        public string UrlFile;
        [DataMember]
        public int Total;
        [DataMember]
        public DateTime BeginDate;
        [DataMember]
        public DateTime? FinishDate;
        [DataMember]
        public int UserId;
        [DataMember]
        public bool HasError;
        [DataMember]
        public string UrlFileError;
    }
}
