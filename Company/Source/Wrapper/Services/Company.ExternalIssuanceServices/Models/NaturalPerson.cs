using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.ExternalIssuanceServices.Models
{
    public class NaturalPerson
    {
        public string TypeCardCode { get; set; }
        public string IdCardNo { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MotherLastName { get; set; }
        public int MaritalStatusCode { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int EconomicActivityCode { get; set; }
        public int NationalityCode { get; set; }
    }
}
