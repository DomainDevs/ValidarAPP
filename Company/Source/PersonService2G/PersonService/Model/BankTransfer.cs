using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Model
{
    public class BankTransfer
    {
        public int Id { get; set; }
        public int IndividualId { get; set; }
        public int BankId { get; set; }
        public string BankDescription { get; set; }
        public string BankBranchId { get; set; }
        public string BankBranchDescription { get; set; }
        public string BankSquare { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public string CityDescription { get; set; }
        public int CountryId { get; set; }
        public string CountryDescription { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountTypeDescription { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyDescription { get; set; }
        public string PaymentBeneficiary { get; set; }
        public string AccountNumber { get; set; }
        public bool ActiveAccount { get; set; }
        public bool DefaultAccount { get; set; }
        public bool IntermediaryBank { get; set; }
    }
}
