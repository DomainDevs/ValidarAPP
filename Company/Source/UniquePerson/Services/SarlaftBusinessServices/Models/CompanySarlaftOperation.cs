using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{
    [DataContract]
    public class CompanySarlaftOperation 
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int SarlaftId { get; set; }
        [DataMember]
        public string ProductNum{ get; set; }
        [DataMember]
        public decimal ProductAmt { get; set; }
        [DataMember]
        public string Entity { get; set; }
        [DataMember]
        public CompanyOperationType CompanyOperationType { get; set; }
        [DataMember]
        public CompanyProductType CompanyProductType { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public int StateId { get; set; }
        [DataMember]
        public int CityId { get; set; }

    }
}
