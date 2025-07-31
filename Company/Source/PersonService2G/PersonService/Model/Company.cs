using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Model
{
    public class Company
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string DocumentNumber { get; set; }
        public int CountryId { get; set; }
        public int? VerifyDigit { get; set; }
        public string Role { get; set; }
        public int CompanyType { get; set; }
        public int AssociationType { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Email> Emails { get; set; }
        public List<Phone> Phones { get; set; }
        public int EconomicActivityId { get; set; }
        public string CheckPayable { get; set; }
        public IdentificationDocument IdentificationDocument { get; set; }
    }
}
