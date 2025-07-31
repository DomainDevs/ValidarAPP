using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
    [DataContract]
    public class InternationalOperationDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int SarlaftId { get; set; }
        [DataMember]
        public string ProductNum { get; set; }
        [DataMember]
        public decimal ProductAmt { get; set; }
        [DataMember]
        public string Entity { get; set; }
        [DataMember]
        public int OperationTypeId { get; set; }
        [DataMember]
        public string OperationDescription { get; set; }
        [DataMember]
        public int ProductTypeId { get; set; }
        [DataMember]
        public string ProductDescription { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public int StateId { get; set; }
        [DataMember]
        public int CityId { get; set; }
        [DataMember]
        public int Status{ get; set; }
    }
}
